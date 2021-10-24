#include "servo.h"

// Timer used to control PWM signal 4 motors
void timer0_init()
{
	//Mode: CTC
	TCCR0A |= (1 << WGM01);
	//Prescaler: 8
	TCCR0B |= (1 << CS01);
	//See documentation for math
	OCR0A = 19;
	// Enable interrupt
	TIMSK0 |= (1 << OCIE0A);
}

void servo_init()
{
	SERVO_BASE_DDR |= (1<<SERVO_BASE);
	SERVO_SHOULDER_DDR |= (1<<SERVO_SHOULDER);
	SERVO_ELBOW_DDR |= (1<<SERVO_ELBOW);
	SERVO_WRIST_DDR |= (1<<SERVO_WRIST);
	SERVO_CAM_DDR |= (1<<SERVO_CAM);
	timer0_init();
	sei();
}

int convert_degrees_to_motorsteps(unsigned char degree)
{
	return (degree * 10UL) / 9UL;
}

int convert_motorsteps_to_degrees(int motorsteps)
{
	return (motorsteps * 9UL) / 10UL;
}

static volatile unsigned int pwm_cycle = 0;
static volatile unsigned char motor_vals[] = { ((SERVO_BASE_START * 10UL) / 9UL), ((SERVO_SHOULDER_START * 10UL) / 9UL), ((SERVO_ELBOW_START * 10UL) / 9UL), ((SERVO_WRIST_START * 10UL) / 9UL)};

ISR(TIMER0_COMPA_vect)
{
	// End of cycle
	if(pwm_cycle++ > 2000)
	{
		SERVO_BASE_PORT &= ~(1<<SERVO_BASE);
		SERVO_SHOULDER_PORT &= ~(1<<SERVO_SHOULDER);
		SERVO_ELBOW_PORT &= ~(1<<SERVO_ELBOW);
		SERVO_WRIST_PORT &= ~(1<<SERVO_WRIST);
		SERVO_CAM_PORT &= ~(1<<SERVO_CAM);
		pwm_cycle = 0;
	}
	//1750 to 2000 => Control pulse
	if(pwm_cycle > 1750)
	{
		if(pwm_cycle > 1950 - motor_vals[0])
		{
			SERVO_BASE_PORT |= (1<<SERVO_BASE);
		}
		if(pwm_cycle > 1950 - motor_vals[1])
		{
			SERVO_SHOULDER_PORT |= (1<<SERVO_SHOULDER);
		}
		if(pwm_cycle > 1950 - motor_vals[2])
		{
			SERVO_ELBOW_PORT |= (1<<SERVO_ELBOW);
		}
		if(pwm_cycle > 1950 - motor_vals[3])
		{
			SERVO_WRIST_PORT |= (1<<SERVO_WRIST);
		}
		if(pwm_cycle > 1845)
		{
			SERVO_CAM_PORT |= (1<<SERVO_CAM);
		}
	}
	
}

unsigned int servo_get(unsigned char id)
{
	return convert_motorsteps_to_degrees(motor_vals[id]);
}

unsigned char servo_allMotorsAtTarget(unsigned char targetDagrees[])
{
	for (unsigned char i = 0; i < 4; i++)
	{
		if(motor_vals[i] != convert_degrees_to_motorsteps(targetDagrees[i]))
			return 0;
	}
	
	return 1;
}

void servo_moveOneStepToTarget(unsigned char targetDagrees[])
{
	for (unsigned char i = 0; i < 4; i++)
	{
		unsigned char val = convert_degrees_to_motorsteps(targetDagrees[i]);
		if(val < motor_vals[i])
			motor_vals[i]--;
		else if(val > motor_vals[i])
			motor_vals[i]++;
	}
}

void servo_moveToStartPosition(void)
{
	unsigned char startDegrees[] = {SERVO_BASE_START, SERVO_SHOULDER_START, SERVO_ELBOW_START, SERVO_WRIST_START};
	while (!servo_allMotorsAtTarget(startDegrees))
	{
		servo_moveOneStepToTarget(startDegrees);
		_delay_ms(DELAY_TIME);
	}
}