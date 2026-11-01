# Approved loop examples

1. Pyramid Generator — nested `for` loops, rows and symbol inputs, centered text output.
2. Sum of Digits — a `while` loop extracts and adds decimal digits.
3. Prime Factorization — `while` loops repeatedly divide a positive integer into prime factors.
4. Number Guessing Game — `do…while` processes guesses until correct or the attempt limit is reached.
5. Quiz Retry Simulator — `do…while` processes answers until correct or the attempt limit is reached.

All five use the existing Input/Result panels, field styles, result typography, canonical GET reset links and server-side validation. Calculations remain in C#. Game rounds travel in protected hidden form values so native POST works without a database or JavaScript. Keep attempts bounded and reject altered round data.
