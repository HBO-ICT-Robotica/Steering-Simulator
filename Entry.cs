﻿namespace Steering {
	class Entry {
		static void Main(string[] args) {
            Love.Boot.Init(new Love.BootConfig() {
                WindowWidth = 1280,
                WindowHeight = 720,
                WindowDisplay = 0,
            });

			Love.Boot.Run(new Program());
		}
	}
}
