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
		
		for (unsigned char i = 0;i < 20; i++)
		{
			if(scanf("%c", &tbuffer[i]) != 1)
			{
				uart_error();
				printf("\n\rError reading letter\n\r");
				
				continueFlag = 1;
				break;
			}
			
			if(i == 0)
			{
				//Reset Position
				if(tbuffer[0] == 'R')
				{
					printf("esetting motors...\n\r");
					servo_moveToStartPosition();
					printf("Reached destination.\n\r");
					printf("[%3u][%3u][%3u][%3u]\n\r", servo_get_base(), servo_get_shoulder(), servo_get_elbow(), servo_get_wrist());
					
					continueFlag = 1;
					break;
				}
				//Get Position (Print position)
				else if(tbuffer[0] == 'G')
				{
					printf("\n\r[%3u][%3u][%3u][%3u]\n\r", servo_get_base(), servo_get_shoulder(), servo_get_elbow(), servo_get_wrist());
					
					continueFlag = 1;
					break;
				}
				//Error
				else if(tbuffer[0] != '[')
				{
					printf("\n\rError reading letter\n\r");
					continueFlag = 1;
					break;
				}
			}
			
			//Invalid number or char
			if(tbuffer[i] != '[' && tbuffer[i] != ']' && (tbuffer[i] < 48 || tbuffer[i] > 57))
			{
				printf("\n\rError Invalid number or character\n\r");
				continueFlag = 1;
				break;
			}
		}
		
		if(continueFlag)
			continue;
		
		//Check length data
		if(strlen(tbuffer) != 20)
		{
			printf("\n\rError in Buffer Length\n\r");
			continue;
		}
			
		//Test Brackets Positioning
		if(tbuffer[0] != '[' || tbuffer[5] != '[' || tbuffer[10] != '[' || tbuffer[15] != '['
		|| tbuffer[4] != ']' || tbuffer[9] != ']' || tbuffer[14] != ']' || tbuffer[19] != ']')
		{
			printf("\n\rError in Bracket Position\n\r");
			continue;
		}
	
		//Read Values and save them in Array
		sscanf(tbuffer,"%*c%3u%*c%*c%3u%*c%*c%3u%*c%*c%3u%*c",&tDegrees[0], &tDegrees[1], &tDegrees[2], &tDegrees[3]);
		
		if((tDegrees[0] < SERVO_BASE_MIN || tDegrees[0] > SERVO_BASE_MAX) || (tDegrees[1] < SERVO_SHOULDER_MIN || tDegrees[1] > SERVO_SHOULDER_MAX) 
		|| (tDegrees[2] < SERVO_ELBOW_MIN || tDegrees[2] > SERVO_ELBOW_MAX) || (tDegrees[3] < SERVO_WRIST_MIN || tDegrees[3] > SERVO_WRIST_MAX))
		{
			printf("\n\rError in Limits\n\r");
			continue;
		}
		
		move_motors_to_target();
		
		printf("\n\rReached destination.\n\r");
    }
}

