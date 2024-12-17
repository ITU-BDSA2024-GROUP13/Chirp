% State which LLM(s) were used during development of your project.
% In case you were not using any, just state so.
% In case you were using an LLM to support your development, briefly describe when and how it was applied.
% Reflect in writing to which degree the responses of the LLM were helpful.
% Discuss briefly if application of LLMs sped up your development or if the contrary was the case.


The LLMs which were used throughout the development process were: ChatGPT, GitHub CoPilot and Codium.

All three LLMs were used mainly for debugging. For the generation of most of the documentation, it was only ChatGPT which was used.

As a rule, whenever any of the LLMs generated any code which was used, it was co-authored in the commit where that piece of code was included.
If an LLM was used simply for sparring to find the root cause of a bug, it was not included in the co-author message.

In terms of the value of there responses it varied. Sometimes, it was a small human error which was overseen. In other more complex cases, it required a greater understanding of the program which the LLMs, especially ChatGPT lacked. In these situations, the LLMs which are built in to text editors, GitHub CoPilot and Codium, were able to gather more information, but were still not always able to solve errors.
This may have lead to some spirals throughout the development process and over-relying on an LLM to find a solution.