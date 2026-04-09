---
on:
  issues:
    types: [opened]
permissions: read-all
safe-outputs:
  add-comment:
  add-labels:
---

# Issue Clarifier & Triage

When a new issue is opened:
1) Summarize the issue in 1–2 sentences.
2) If key info is missing (steps to reproduce, expected vs actual, environment), ask 3 concise questions.
3) Apply labels:
   - bug if it looks like a defect
   - docs if it’s documentation-related
   - question if it’s unclear
Output:
- a single comment on the issue
- labels applied
