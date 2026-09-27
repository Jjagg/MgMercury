using System;
using System.IO;

Directory.SetCurrentDirectory(AppContext.BaseDirectory);

using var game = new Demo.Game1();
game.Run();
