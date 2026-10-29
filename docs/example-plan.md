# Example planning

## Input/Output

### Example 1 — ID Card Generator (confirmed)

- User enters their details and selects a photo, then chooses Generate ID to display a card preview on the page.
- Preview only: no download button.
- The photo stays local to the browser. Do not upload it, include it in form submissions, save it to a database/server folder, or persist it in localStorage/IndexedDB.
- JavaScript handles the local photo preview; C# validates and formats the text details.
- Clear releases the temporary photo reference and resets the form and preview. Page reload resets the preview state; do not restore it from the application's navigation cache.
- This is an approved concept awaiting implementation. Keep the current example placeholder until implementation is requested.

### Example 2 — BMI Calculator (confirmed)

- Input: weight in kilograms and height in centimeters.
- Output: calculated BMI value, using height converted to meters.
- This is an approved concept awaiting implementation; classification or health advice has not been requested.

### Example 3 — Temperature Converter (confirmed)

- Convert an entered temperature between temperature units; the exact unit controls remain an implementation choice.
- This is an approved concept awaiting implementation.

### Example 4 — Time Unit Converter (confirmed)

- User enters a numeric value and selects separate From and To units.
- Both unit selectors offer seconds, minutes and hours.
- Output: the equivalent value in the selected destination unit. For example, 3 hours converts to 10,800 seconds.
- This is an approved concept awaiting implementation.

### Example 5 — Character and Word Counter (confirmed)

- User enters text or a paragraph in a multiline text box.
- Output: character count and word count for the entered text.
- This is an approved concept awaiting implementation. Details such as whether spaces count as characters remain to be finalized.

All five Input/Output concepts are now selected. These planning decisions do not implement the examples or replace their current placeholders.
