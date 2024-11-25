# QuestPDF performance scaling issue
This repository contains a simple console application that I believe showcases certain issues with the way QuestPDF scales.

It contains two implementations of the same document:
 - `RegularDocument` represents the initial implementation, using multiple columns/decorations to render the document.
 - `DynamicDocument` represents the re-implementation of the document using a single dynamic component, in an attempt to lower the memory usage.