fa-swarm-3d
===========

Twin stick shooter built with Unity set in the Funkadelic Astronaut universe.

## Getting Started
1) Download Unity.
2) Clone this Repo.
3) Browse to the repo in Unity Hub
4) Open the project and load the "Demo" scene in the "Scenes" folder from the Hierarchy.
5) Run the scene.

## Git stuff
You might need to install git lfs if you don't have it installed. Git for windows seems to come with it.

See: https://thoughtbot.com/blog/how-to-git-with-unity

## Coding Pattersn

### Events
Events should take the form of `[Action]Event`. For example: `DamageEvent`.

Events should be in the present tense. eg. `Damage` vs `Damaged`. (Use `Damage`).

A helpful hint is that the event should read like this: "On [Action] Event".

The intention was to name all events using the form `On[Action]Event` but this allows us to ommit `On` from all the names.
