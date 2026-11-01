# Shared example layout verification

Validated on November 2, 2026. The four working Input/Output examples and all five Nested If…Else examples use the approved Example 4 panel layout. The other eleven slots remain unimplemented templates.

## Behavior and layout

- All nine working examples: side-by-side Input and Result panels at 1280×720; stacked panels at 390×844 and 320×568; no horizontal workspace overflow. Phone fields are at least 44px tall.
- Short landscape (844×390): the login example scrolls within the application and clears the taskbar.
- Temperature: 25°C converts to 77°F, Swap reuses 77 as the next input, and Clear restores defaults and empty output. Native POST Swap also works without JavaScript.
- ID generator: text preview and Clear toggle the card and shared empty state together. The photo input has no name and remains excluded from native submissions.
- Text counter: “Hello student” returns 13 characters and 2 words. Time conversion: 23 hours returns 82,800 seconds with its formula.
- Decision examples: existing branch, invalid-input, boundary and antiforgery tests pass. Native results retain automatic keyboard focus; edits hide the prior output and restore the empty state.
- Browser Back/Forward, keyboard focus, search opening/Escape dismissal, minimize/restore, maximize/taskbar clearance and close-to-desktop were checked in the browser.
- Existing reduced-motion rules remain intact; the shared panels add no animation.

## Commands

```powershell
dotnet build 241611JalopFinalAssignment1.sln
node --test tests/navigation.test.cjs tests/input-output.test.cjs tests/nested-if-else.test.cjs
dotnet run --project 241611JalopFinalAssignment1 --launch-profile http --no-build
```

The build passes with zero warnings or errors. All 17 test cases pass. Integration tests require the local app on port 5196 (or `SHOWCASE_BASE_URL`). The application was rerun after each implementation batch and left running for review.
