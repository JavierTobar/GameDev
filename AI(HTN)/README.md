# Summary

You need to steal the treasure chest from a monster that's guarding it and return back to your spawning location.

The monster uses [hierarchical task network](https://en.wikipedia.org/wiki/Hierarchical_task_network) to determine its behavior. 

If you are within visible range, it will throw either rocks or crates at you. 

If you are close to it, it might even melee you, turning red before doing so.

You can only take 2 hits before being eliminated. Luckily you have 10 seconds of shield time that you toggle on/off with space bar.

There are also some rats that are meant to simulate [steering behavior](https://www.gamedev.net/blogs/entry/2264855-steering-behaviors-seeking-and-arriving/).

However, some forces still need fine tuning in order to appear more realistic.


### This project showcases an approach to automated planning, i.e. hierarchical task network (AI), as well as steering behavior.

Preview of gameplay
![Gameplay](41kYLBGKW0.gif)
