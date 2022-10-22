# BODOT
A simple little build tool for godot projects.
Bodot makes it easy to export via the command line, with helpful utilities for tracking
version info as well as automatically zipping your exports.

Use the godot editor to configure your exports before using bodot.

## Getting Started
Download the bodot binary and add it to your PATH.

From within your godot project directory, run
```
bodot --init
```
Follow the directions to setup your build folder as well as pointing bodot to the godot binary you want
it to use.

To export your project, simply run
```
bodot --build
```
This will create exports based on all your available export presets **NOT STABLE**

## All Commands
Initialize config
```
bodot --init
```

To view current config info
```
bodot --info
```

To build all available exports
```
bodot --build|-b
```

To build and zip exported binaries
```
bodot --build|-b --zip|-z
```

To set config options
```
bodot --config [OPTION] [VALUE]
``` 

