# Room Joiner (BepInEx)

This is a simple BepInEx plugin for Gorilla Tag that adds a small in game menu for joining rooms by name and enabling WASD fly.
When loaded through BepInEx, the mod creates a basic GUI window where you can type a room code and join it directly, disconnect from the current room, and toggle WASD controls. The movement is handled client side by overriding the players rigidbody velocity and applying manual position which updates each frame. Mouse input is used for camera rotation while flying.

The compiled code can be inspected or reversed using tools like ILSpy or dnSpy.
