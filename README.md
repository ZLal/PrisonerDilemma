# Prisoner Dilemma

The prisoner's dilemma is a game theory thought experiment.

This project is humble recreation attempt inspired by Prof. Robert Axelrod's experiment.

[Prisoner's Dilemma Wiki](https://en.wikipedia.org/wiki/Prisoner's_dilemma)

### Dependencies
| Support       | Package                       | Version        |
| ------------- | ----------------------------- | -------------- |
| Python        | IronPython                    | 3.4.1          |
| C# script     | Microsoft.CodeAnalysis.CSharp | 4.9.2          |
| Javascript    | NiL.JS                        | 2.5.1677       |
| Http          | wWw.HttpParser                | 2021.11.14.825 |

### Score

|               | Cooperate     | Defect |
| ------------- | ------------- | ------ |
| **Cooperate** | 3, 3          | 0, 5   |
| **Defect**    | 5, 0          | 1, 1   |

### Loading script
BotFactory &rarr; BotAdapter  &rarr; ScriptLoader  &rarr; BotScript
