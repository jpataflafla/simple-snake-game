# "Snake" game

## Overview
### Main objectives
A simple snake game created as part of a pre-employment test assignment for a Unity developer position.

### Brief description of the game concept
A simple game, based on well known comcept of "Snake" game ([Snake game - Wikipedia](https://en.wikipedia.org/wiki/Snake_(video_game_genre))),  where the player is responsible for changing the direction of the snake's head. The player must be careful not to cause the snake to collide with itself or other obstacles, which becomes more difficult with each elongation of the snake as a result of eating an edible item. According to the cited link, there are at least two popular versions of the game. This implementation is for the single-player version.


### Visual design assumptions
- **Visual Simplicity:** This is a POC project, so the game adopts a minimalist look, using basic shapes to ensure clarity and ease of understanding. Neon and distracting colors will be rather avoided to make the gameplay comfortable for the player.

### View
- **Perspective:** The game will use a "top-down" perspective to maintain a classic look and style.

### Game mechanics
- The snake will be composed of segments. They will follow one after the other, so each of them will take the same path after some time.
- The snake will always move, the player changes the direction of its head.
- There will be randomly placed interactive elements on the map that have different properties/effects.
  Types of elements included in POC:
  - **Edible items:** When the snake touches food, it eats it. The snake's body length increases by one segment and the player receives a point (1p).
  - **Inedible item:** Shortens the snake by one segment and takes away one point (-1p).
  - **Slow down element:** Temporarily slows down the snake movements.
  - **Speed up element:** Temporarily speed up the snake movements.
  - **Reverse the snake:** The head is swapped with the tail, and the direction of movement is reversed.

- There should be a score on the screen showing how many "edible items" the snake has eaten in the current round of the game.

- **The game field will be "looped":** e.g., when the snake goes beyond the right border, it will appear from the left. This will work the same way for the up-down direction.

- When the snake collides with itself - the game will restart.


### Target Platform
  The primary target platform will be Windows, but the game will be fairly easy to port to other platforms.

### Other important assumptions
- This is a POC / UX demo that should be delivered quickly, not overengineered.
- The purpose of the game is to show gameplay/ux, not visuals.
- The code and game will evolve later in a series of rapid gameplay prototypes (which is beyond the scope of this task). Potential customers may want to experiment with numerical parameters (probabilities of "edible elements," duration and severity of effects, speed of play, etc.). They may also ask to add new effects or interactive elements. 
- The game should be written in a way that to be fairly easy to port to other platforms.
- The basic mechanics of the snake game will remain unchanged and will never lose its initial concept or transform into any other type of game.

## Unity implementation and considerations
This Unity project serves as a prototype for a classic Snake game, with a focus on simplicity, flexibility, and extensibility. Below are key insights into the implementation, bearing in mind the prototype nature of the game.

### Used Assets and Packages
- **Canva**: Some basic sprites were created using Canva.
- **Pixabay Sounds**: Free-to-use sounds from Pixabay to make game more playable even at prototype stage.
- **DOTween**: Adds smooth animation and tweening capabilities, enhancing the visual prototype.
- **New Unity Input System**: This Unity prototype for a classic Snake game implements the new Unity Input System for enhanced input handling. Prioritizing simplicity, flexibility, and adaptability across diverse testing platforms, the UI features on-screen buttons for intuitive control and usability, especially on touch screens. The Unity Input System ensures compatibility with WASD, arrow keys, and other input methods, enhancing the overall user experience.

## Assets, Packages, custom solutions for future
- **Localization Asset:** Localization package (i.e. I2 Localization or custom solution) to easily store and manage texts outside of the code, supporting various language versions etc.
- **Dependency Injector (like Zenject):** In the future, implementing a robust Dependency Injection (DI) framework such as Zenject could be considered. This would facilitate effective dependency management, making it easier for testing, interface changes and overall project scalability or the ability to quickly swap implementations with more scenes sharing scripts in the project.

### Design Considerations

- **Flexible Board Configuration**: The prototype is designed with a square board featuring checkerboard tiles, easily adjustable in the inspector for experimentation. This includes parameters like map resolution, allowing for more or fewer squares for the snake to traverse. Speed, time of speed effects, etc., every important parameter of the game, is there to simplify the testing process. It should also be quite easy to create a Settings screen/scene where the player could change parameters to tweak the game to their liking — which could be very useful in a prototype testing scenario.
- **Game Manager Dependencies**: All parameters are exposed in the inspector on the game manager object, allowing for quick adjustments. Most of the interchangeable scripts, such as the audio manager, score manager, board generation, etc., are decoupled and implemented with interfaces. This approach allows for fast changes during the prototyping phase and makes it easier to integrate the game into a larger application.

![Local Image](/gameScreenRec/Screen_snake.png)
![Local Image](/gameScreenRec/gameView.png)

## Conclusion

In conclusion, this Snake game prototype in Unity provides a solid foundation for experimentation and iteration. The emphasis on simplicity and flexibility, coupled with thoughtful design considerations, ensures a prototype that can easily adapt to different configurations. As the project progresses, further considerations, such as adopting a DI framework, can be explored to maintain clean and scalable code. 
