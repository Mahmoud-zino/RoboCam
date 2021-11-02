#include "main.h"
#include <string.h>

char tbuffer[25];

void clear_buffer(char *tbuffer)
{
	for (unsigned char i=0; i < 25; i++)
	{
		tbuffer[i] = '\0';
	}
}

unsigned char tDegrees[5] = {SERVO_BASE_START, SERVO_SHOULDER_START, SERVO_ELBOW_START, SERVO_WRIST_START, 0};

void move_motors_to_target()
{
	while(!servo_allMotorsAtTarget(tDegrees))
	{
		servo_moveOneStepToTarget(tDegrees);
		_delay_ms(DELAY_TIME);
	}
}

int main(void)
{
	uart_init();
	servo_init();
	
	_delay_ms(100);
	
	sei();
	
	servo_moveToStartPosition();
	
	unsigned char continueFlag = 0;	

	while(1)
    {
		clear_buffer(tbuffer);
		continueFlag = 0;
		
		//Read Command
		for (unsigned char i = 0;i < 18; i++)
		{
			if(scanf("%c", &tbuffer[i]) != 1)
			{
				uart_error();
				
				continueFlag = 1;
				break;
			}
			
			if(i == 0)
			{
				//Reset Position
				// for test propuses
				if(tbuffer[0] == 'R')
				{
					printf("esetting motors.\n\r");
					servo_moveToStartPosition();
					
					continueFlag = 1;
					break;
				}
				
				//Get Position (Print position)
				// for test propuses
				else if(tbuffer[0] == 'G')
				{
					printf("[%3u;%3u;%3u;%3u;]\n\r", servo_get(0), servo_get(1), servo_get(2), servo_get(3));
					
					continueFlag = 1;
					break;
				}
				//Error
				else if(tbuffer[0] != '[')
				{
					continueFlag = 1;
					break;
				}
			}
			
			//Invalid number or char
			if(tbuffer[i] != '[' && tbuffer[i] != ']' && tbuffer[i] != ';' && (tbuffer[i] < 48 || tbuffer[i] > 57))
			{
				continueFlag = 1;
				break;
			}
		}
		
		if(continueFlag)
			continue;
		
		//Check length data
		if(strlen(tbuffer) != 18)
		{
			continue;
		}
			
		//Test Brackets Positioning
		if(tbuffer[0] != '[' || tbuffer[4] != ';' || tbuffer[8] != ';' || tbuffer[12] != ';' || tbuffer[16] != ';'|| tbuffer[17] != ']')
		{
			continue;
		}
			
		//Read Values and save them in Array
		sscanf(tbuffer,"%*c%3u%*c%3u%*c%3u%*c%3u%*c%*c",&tDegrees[0], &tDegrees[1], &tDegrees[2], &tDegrees[3]);
		
		if((tDegrees[0] < SERVO_BASE_MIN || tDegrees[0] > SERVO_BASE_MAX) || (tDegrees[1] < SERVO_SHOULDER_MIN || tDegrees[1] > SERVO_SHOULDER_MAX) 
		|| (tDegrees[2] < SERVO_ELBOW_MIN || tDegrees[2] > SERVO_ELBOW_MAX) || (tDegrees[3] < SERVO_WRIST_MIN || tDegrees[3] > SERVO_WRIST_MAX))
		{
			continue;
		}
		
		move_motors_to_target();
    }
}

