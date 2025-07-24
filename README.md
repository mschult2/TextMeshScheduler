# Summary
The Unity TextMeshoPro method `SetText()` is quite expensive. Same with `.text`. Writing 70 characters takes 3 milliseconds on my Android mobile device. Even if you write to multiple small text meshes in a single frame, they still get bunched together into one expensive Canvas prerender operation. This is even with Autosize, Rich Text, Parse Escape Characters, Text Wrapping, and Kern disabled.
So I made a simple component called TextMeshScheduler which collects all of the calls to `SetText()` and distributes them across multiple frames. 
Tested on Unity 6 (6000.0.51f1).

# Usage
Add the TextMeshScheduler component to your scene. Then invoke this extension method on TMP_Text, TextMeshProUGUI, or TextMeshPro:

```sh
tmp_text.ScheduleText("John Smith");
```

Then make every header and field its own text mesh; no monolithic text meshes.

And for best performance:
* Disable autosize
* Set Text Wrapping Mode to None
* Disable Rich Text
* Disable Parse Escape Characters
* Set Font Features to Nothing
