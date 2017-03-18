# BTreeInterruptGroup

Interrupts each tree root of an specified list of groups when the BTreeInterruptGroup's children matches an state. An interrupted group is locked from running until all child states are not an matching state.

## Parameters

- Interrupt Root Groups - Names of groups belonging to the tree root to be interrupted
- Interrupt On State - States of children that should interrupt