#ifndef MAIN_H_
#define MAIN_H_

#ifndef F_CPU
#define F_CPU 16000000UL
#endif

#ifndef HIGH
#define HIGH 0xFF
#endif

#ifndef LOW
#define LOW 0x00
#endif

#include <stdio.h>
#include <stdlib.h>
#include <avr/io.h>
#include <avr/interrupt.h>
#include <util/delay.h>
#include <avr/wdt.h>

#include "init.h"
#include "uart.h"
#include "servo.h"

int main(void);

#endif