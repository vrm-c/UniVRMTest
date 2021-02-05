# UniVRMTest

[日本語](README.ja.md)

UniVRMTest is for [UniVRM](https://github.com/vrm-c/UniVRM) samples and build tests.

# License

The project is released under the [MIT License](License.txt).

## VRMViewer

VRMViewer is a runtime VRM loader for checking VRM model's information and runtime behaviors.

## Download

1. Go to the [releases page](https://github.com/vrm-c/UniVRMTest/releases)
1. Download the latest version ``VRMViewer_v0.xx.x``

## How to load a VRM file

1. Run ``VRMViewer_v0.xx.x.exe``
1. Click the VRM Model button
1. Select a VRM file from the local directory

<img src="doc/VRMViewerInterface.png" width="800">

## ToDo

* [x] Model information
* [x] Command line arguments
* [x] Switch options for T-Pose / BVH Motion
* [x] Model LookAt: straight ahead / target / camera
    * [x] VRMLookAtBoneApplyer
    * [x] VRMLookAtBlendShapeApplyer
* [ ] BlendShape
    * [x] AIUEO
    * [x] AutoBlink 
    * [ ] Next, Prev
* [x] Expression list
* [x] Model rendering in first-person mode
* [ ] Vertex count

