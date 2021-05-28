/* -----------------------------------------
 * Liebherr Lehrlingsausbildung
 * www.liebherr.com
 * -----------------------------------------
 *    Hardware: Megacard (ATmega16)
 * -----------------------------------------
 *     Version: 1.0 Release
 *      Author: G.Raf
 * Description:
 *   Function file for uart library
 * -----------------------------------------
 */


#include "uart.h"

// Initialize FILE stream
#if !defined(UARTRXCIE) && !defined(UARTTXCIE) && !defined(UARTTXCIE)
    static FILE std_uart = FDEV_SETUP_STREAM(uart_putchar, uart_getchar, _FDEV_SETUP_RW);
#elif !defined(UARTRXCIE) && (defined(UARTTXCIE) || defined(UARTTXCIE))
    static FILE std_uart = FDEV_SETUP_STREAM(null, uart_getchar, _FDEV_SETUP_READ);
#elif defined(UARTRXCIE) && !defined(UARTTXCIE) && !defined(UARTTXCIE)
    static FILE std_uart = FDEV_SETUP_STREAM(uart_putchar, null, _FDEV_SETUP_WRITE);
#endif

//  +---------------------------------------------------------------+
//  |                       UART initialization                     |
//  +---------------------------------------------------------------+
void uart_init(void)
{   
    // Check which bit sampling mode should be activated
    #if USE_2X
    UCSR0A |= (1<<U2X0);      // Setup 8 samples/bit
    #else
    UCSR0A &= ~(1<<U2X0);     // Setup 16 samples/bit
    #endif

    UBRR0H = (0x0F & UBRRH_VALUE);   // Calculated through setbaud.h
    UBRR0L = UBRRL_VALUE;            // Calculated through setbaud.h

    unsigned char SETREG = (1<<UMSEL01);  // Activate URSEL (normally in register UCSRC)
    
    SETREG |= (0x06 & ((UART_DATASIZE - 5)<<1));    // Setup data size
    
    #if UART_PARITY > 0
        SETREG |= (0x30 & ((UART_PARITY + 1)<<4));  // Parity Mode
    #endif
    
    #if UART_STOPBITS > 1
        SETREG |= (0x08 & (UART_STOPBITS<<3));      // Setup stop bits
    #endif
    
    UCSR0C = SETREG;                 // Write SETREG settings to UCSRC
    UCSR0B = (1<<RXEN0) | (1<<TXEN0);  // Activate UART transmitter and receiver

    // Interrupt control
    
    // Receiver interrupt setup
    #ifdef UARTRXCIE
        UCSR0B  |= (1<<RXCIE0);
        sei();
    #endif

    // Transmitter interrupt setup
    #if defined(UARTTXCIE) && !defined(UARTUDRIE)
        UCSR0B  |= (1<<TXCIE0);
        sei();
    #endif

    // Transmitter interrupt setup
    #if !defined(UARTTXCIE) && defined(UARTUDRIE)
        UCSR0B  |= (1<<UDRIE0);
        sei();
    #endif

    // Setup stream to stdout
    #if !defined(UARTTXCIE) && !defined(UARTUDRIE)
        stdout = &std_uart;
    #endif
    
    // Setup stream to stdin
    #if !defined(UARTRXCIE)
        stdin = &std_uart;
    #endif
}

#if !defined(UARTTXCIE) && !defined(UARTUDRIE)

    //  +---------------------------------------------------------------+
    //  |                   UART send character                         |
    //  +---------------------------------------------------------------+
    //  | Parameter:    data            ->  ASCII character             |
    //  |               stream (ptr)    ->  Datastream                  |
    //  |                                                               |
    //  |    Return:    0x00    ->  Transmission complete               |
    //  +---------------------------------------------------------------+
    int uart_putchar(char data, FILE *stream)
    {
        // Wait until last transmission completed
        while(!(UCSR0A & (1<<UDRE0)))
            asm volatile("NOP");
    
        UDR0 = data; // Write data to transmission register
    
        // C99 functions needs an int as a return parameter
        return 0;   // Return that there was no fault
    }

#endif

#if !defined(UARTRXCIE)

    //  +---------------------------------------------------------------+
    //  |                   UART receive character                      |
    //  +---------------------------------------------------------------+
    //  | Parameter:    stream (ptr)    ->  Datastream                  |
    //  |                                                               |
    //  |    Return:    0x??    ->  data/NUL                            |
    //  +---------------------------------------------------------------+
    int uart_getchar(FILE *stream)
    {
        // Wait until data has been received
        while(!(UCSR0A & (1<<RXC0)));
    
        // Ckeck if there was an error
        // Frame error
        if(UCSR0A & (1<<FE0))
        {
            UDR0;        // Clear UART data register
            return 0;   // Return NUL
        }
        // Data Overrun error
        else if(UCSR0A & (1<<DOR0))
        {
            UDR0;        // Clear UART data register
            return 0;   // Return NUL
        }
        // Parity error
        else if(UCSR0A & (1<<UPE0))
        {
            UDR0;        // Clear UART data register
            return 0;   // Return NUL
        }
    
        #ifdef UART_RXC_ECHO
            // Write UART data register to data variable
            char data = UDR0;
        
            // Send echo of received data to UART 
            uart_putchar(data, stdout);
        
            return data;    // Return received data
        #else
            return UDR0; // Return UART data register
        #endif
    }

#endif

#if !defined(UARTRXCIE)

    //  +---------------------------------------------------------------+
    //  |                   UART error handler                          |
    //  +---------------------------------------------------------------+
    void uart_error(void)
    {
        clearerr(stdin);    // Clear error on stream
        getchar();          // Remove character from stream
    }

#endif