# Notes

## Auth

- Il login verrà gestito lato client.
- Il JWT resterà nella secure storage del dispositivo.
- Evito cache/sessioni server-side per mantenere l'API stateless.

## Username

Da creare:

- endpoint per verificare se il profilo é completo
- immaggine di profilo?
- Puoi aggiornare l'username?
- validazione username lato API

## Payments

Da creare:

- endpoint per inizializzare un nuovo pagamento
- valutare gestione webhook separata

## Architecture thoughts

Possibile migrazione futura:

- da Layered a Modular
- separazione auth/payments in servizi indipendenti

Motivazioni:

- migliore scalabilità
- deploy separati
- isolamento responsabilità
