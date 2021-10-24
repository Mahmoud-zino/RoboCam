#ifndef UART_H_
#define UART_H_

#ifndef F_CPU               // System clock
    #define F_CPU 16000000UL
#endif


#ifndef BAUD                // Transmission frequency in bits/s
    #define BAUD 38400UL
#endif


#ifndef UART_DATASIZE       // Setup 5 to 8
    #define UART_DATASIZE 8
#endif


#ifndef UART_PARITY         // Setup 0 = none | 1 = even | 2 = odd
    #define UART_PARITY 0
#endif


#ifndef UART_STOPBITS       // Setup 1 - 2
    #define UART_STOPBITS 1
#endif

#ifndef UART_RXC_ECHO       // Setup if echo occurs on data receiving
    #define UART_RXC_ECHO
#endif

#include <stdio.h>
#include <avr/io.h>
#include <util/setbaud.h>

#if defined(UARTRXCIE) || defined(UARTTXCIE) || defined(UARTUDRIE)
    #include <avr/interrupt.h>
#endif

void uart_init(void);

#if !defined(UARTTXCIE) && !defined(UARTUDRIE)
     int uart_putchar(char c, FILE *stream);
#endif

#if !defined(UARTRXCIE)
     int uart_getchar(FILE *stream);
#endif

#if !defined(UARTRXCIE)
    void uart_error(void);
#endif

#endif /* UART_H_ */