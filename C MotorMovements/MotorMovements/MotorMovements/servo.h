#ifndef SERVO_H_
#define SERVO_H_

#ifndef SERVO_BASE_DDR
#define SERVO_BASE_DDR  DDRB
#endif
#ifndef SERVO_BASE_PORT
#define SERVO_BASE_PORT PORTB
#endif
#ifndef SERVO_BASE_PIN
#define SERVO_BASE_PIN PINB
#endif

#ifndef SERVO_SHOULDER_DDR
#define SERVO_SHOULDER_DDR  DDRB
#endif
#ifndef SERVO_SHOULDER_PORT
#define SERVO_SHOULDER_PORT PORTB
#endif
#ifndef SERVO_SHOULDER_PIN
#define SERVO_SHOULDER_PIN PINB
#endif

#ifndef SERVO_ELBOW_DDR
#define SERVO_ELBOW_DDR  DDRB
#endif
#ifndef SERVO_ELBOW_PORT
#define SERVO_ELBOW_PORT PORTB
#endif
#ifndef SERVO_ELBOW_PIN
#define SERVO_ELBOW_PIN PINB
#endif

#ifndef SERVO_WRIST_DDR
#define SERVO_WRIST_DDR  DDRD
#endif
#ifndef SERVO_WRIST_PORT
#define SERVO_WRIST_PORT PORTD
#endif
#ifndef SERVO_WRIST_PIN
#define SERVO_WRIST_PIN PIND
#endif

#ifndef SERVO_CAM_DDR
#define SERVO_CAM_DDR  DDRD
#endif
#ifndef SERVO_CAM_PORT
#define SERVO_CAM_PORT PORTD
#endif
#ifndef SERVO_CAM_PIN
#define SERVO_CAM_PIN PIND
#endif

#ifndef SERVO_BASE
#define SERVO_BASE     PINB3
#endif
#ifndef SERVO_SHOULDER
#define SERVO_SHOULDER PINB2
#endif
#ifndef SERVO_ELBOW
#define SERVO_ELBOW	   PINB1
#endif
#ifndef SERVO_WRIST
#define SERVO_WRIST    PIND6
#endif
#ifndef SERVO_CAM
#define SERVO_CAM      PIND5
#endif

#ifndef SERVO_BASE_START
#define SERVO_BASE_START 90
#endif
#ifndef SERVO_SHOULDER_START
#define SERVO_SHOULDER_START 135
#endif
#ifndef SERVO_ELBOW_START
#define SERVO_ELBOW_START 150
#endif
#ifndef SERVO_WRIST_START
#define SERVO_WRIST_START 155
#endif

#ifndef SERVO_BASE_MIN
#define  SERVO_BASE_MIN 0
#endif
#ifndef SERVO_BASE_MAX
#define  SERVO_BASE_MAX 180
#endif

#ifndef SERVO_SHOULDER_MIN
#define  SERVO_SHOULDER_MIN 45
#endif
#ifndef SERVO_SHOULDER_MAX
#define  SERVO_SHOULDER_MAX 160
#endif

#ifndef SERVO_ELBOW_MIN
#define  SERVO_ELBOW_MIN 45
#endif
#ifndef SERVO_ELBOW_MAX
#define  SERVO_ELBOW_MAX 160
#endif

#ifndef SERVO_WRIST_MIN
#define  SERVO_WRIST_MIN 45
#endif

#ifndef SERVO_WRIST_MAX
#define  SERVO_WRIST_MAX 180
#endif


#ifndef DELAY_TIME
#define DELAY_TIME 20
#endif

#ifndef F_CPU
#define F_CPU 16000000UL
#endif

#ifndef TIMER0
#define TIMER0
#endif

#include <avr/io.h>
#include <avr/interrupt.h>
#include <util/delay.h>
#include <stdlib.h>

void servo_init();
unsigned char servo_allMotorsAtTarget(unsigned char targetDagrees[]);
void servo_moveOneStepToTarget(unsigned char targetDagrees[]);
void servo_moveToStartPosition(void);
void timer0_init();
unsigned int servo_get(unsigned char id);

#endif