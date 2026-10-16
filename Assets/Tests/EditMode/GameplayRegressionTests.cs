using System;
using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class GameplayRegressionTests
{
    private const BindingFlags InstanceMembers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    /// <summary>
    /// Verifies that a light sink accepts a fully aligned source-to-mirror path and rejects the path
    /// after an intermediate mirror is rotated outside its configured tolerance. The test creates
    /// three linked LightCheck components and expects the recursive sink check to change from true to false.
    /// </summary>
    [Test]
    public void LightPath_RejectsMisalignedMirrorInSourceChain()
    {
        GameObject source = new GameObject("source");
        GameObject mirror = new GameObject("mirror");
        GameObject sink = new GameObject("sink");

        try
        {
            source.transform.position = Vector2.zero;
            mirror.transform.position = new Vector2(2f, 0f);
            sink.transform.position = new Vector2(4f, 0f);

            Type lightCheckType = RuntimeType("LightCheck");
            Component sourceCheck = source.AddComponent(lightCheckType);
            Component mirrorCheck = mirror.AddComponent(lightCheckType);
            Component sinkCheck = sink.AddComponent(lightCheckType);

            SetField(sourceCheck, "nextMirror", mirror);
            SetField(mirrorCheck, "previousMirror", source);
            SetField(mirrorCheck, "nextMirror", sink);
            SetField(sinkCheck, "previousMirror", mirror);

            Invoke(sourceCheck, "setup");
            Invoke(mirrorCheck, "setup");
            Invoke(sinkCheck, "setup");

            Assert.That((bool)Invoke(sinkCheck, "inToleranceFromSource"), Is.True,
                "The configured straight light path should be accepted.");

            mirror.transform.rotation = Quaternion.Euler(0f, 0f, 90f);

            Assert.That((bool)Invoke(sinkCheck, "inToleranceFromSource"), Is.False,
                "The sink check must reject a path when an intermediate mirror is outside tolerance.");
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(source);
            UnityEngine.Object.DestroyImmediate(mirror);
            UnityEngine.Object.DestroyImmediate(sink);
        }
    }

    /// <summary>
    /// Verifies the Temple 2 colored-switch solution. A fixture creates the three switch components,
    /// their required renderer/collider components, and the trap object looked up by the gameplay scripts.
    /// Pressing red, green, then blue is expected to leave all three switches triggered.
    /// </summary>
    [Test]
    public void ColoredSwitches_AcceptRedGreenBlueSequence()
    {
        using (var switches = new ColoredSwitchFixture())
        {
            switches.Press(switches.Red);
            switches.Press(switches.Green);
            switches.Press(switches.Blue);

            Assert.That(switches.Triggered(switches.Red), Is.True);
            Assert.That(switches.Triggered(switches.Green), Is.True);
            Assert.That(switches.Triggered(switches.Blue), Is.True);
        }
    }

    /// <summary>
    /// Verifies that an invalid colored-switch order clears the sequence. The fixture presses green first,
    /// then tries red followed directly by blue; each invalid attempt is expected to leave all switches
    /// untriggered.
    /// </summary>
    [Test]
    public void ColoredSwitches_ResetWhenPressedOutOfOrder()
    {
        using (var switches = new ColoredSwitchFixture())
        {
            switches.Press(switches.Green);

            Assert.That(switches.Triggered(switches.Red), Is.False);
            Assert.That(switches.Triggered(switches.Green), Is.False);
            Assert.That(switches.Triggered(switches.Blue), Is.False);

            switches.Press(switches.Red);
            switches.Press(switches.Blue);

            Assert.That(switches.Triggered(switches.Red), Is.False);
            Assert.That(switches.Triggered(switches.Green), Is.False);
            Assert.That(switches.Triggered(switches.Blue), Is.False);
        }
    }

    /// <summary>
    /// Verifies completion stops an active Temple timer. The test creates inactive Player, Overlay, and
    /// Temple components, supplies the minimal UI objects used by the success overlay, marks the timer as
    /// running, and invokes LevelFinished. It expects the timer to be stopped while retaining positive time.
    /// </summary>
    [Test]
    public void LevelFinished_StopsRunningTimer()
    {
        GameObject playerObject = new GameObject("test player");
        GameObject overlayObject = new GameObject("test overlay");
        GameObject templeObject = new GameObject("test temple");
        GameObject[] overlayObjects = null;
        playerObject.SetActive(false);
        overlayObject.SetActive(false);
        templeObject.SetActive(false);

        try
        {
            Component player = playerObject.AddComponent(RuntimeType("Player"));
            Component overlay = overlayObject.AddComponent(RuntimeType("Overlay"));
            Component temple = templeObject.AddComponent(RuntimeType("Temple"));

            SetField(temple, "player", player);
            SetField(temple, "overlay", overlay);
            SetField(temple, "state", 2);
            SetField(temple, "time", Time.realtimeSinceStartup + 60f);
            SetField(temple, "elapsing", true);
            SetField(temple, "CurrentLevel", 1);

            GameObject continueButton = new GameObject("continue button");
            GameObject nameText = new GameObject("name text");
            GameObject overlayText = new GameObject("overlay text", typeof(Text));
            GameObject pauseButton = new GameObject("pause button");
            GameObject startPanel = new GameObject("start panel");
            overlayObjects = new[] { continueButton, nameText, overlayText, pauseButton, startPanel };
            continueButton.SetActive(false);
            nameText.SetActive(false);
            overlayText.SetActive(false);
            pauseButton.SetActive(false);
            startPanel.SetActive(false);

            SetField(overlay, "continueButton", continueButton);
            SetField(overlay, "nameText", nameText);
            SetField(overlay, "overlayText", overlayText);
            SetField(overlay, "pauseButton", pauseButton);
            SetField(overlay, "startPanel", startPanel);
            SetField(overlay, "winText", "Complete");

            Invoke(temple, "LevelFinished");

            Assert.That(GetField<bool>(temple, "elapsing"), Is.False);
            Assert.That((float)Invoke(temple, "SecondsLeft"), Is.GreaterThan(0f));

        }
        finally
        {
            if (overlayObjects != null)
            {
                foreach (GameObject overlayChild in overlayObjects)
                    UnityEngine.Object.DestroyImmediate(overlayChild);
            }
            UnityEngine.Object.DestroyImmediate(playerObject);
            UnityEngine.Object.DestroyImmediate(overlayObject);
            UnityEngine.Object.DestroyImmediate(templeObject);
        }
    }

    /// <summary>
    /// Verifies map-space room lookup at Temple 2's configured start position. The test loads the level
    /// resource, reads StartRoom/StartRoomX/StartRoomY, registers that room at those coordinates, and expects
    /// RoomAt to return the configured start-room name for the position.
    /// </summary>
    [Test]
    public void RoomAt_FindsConfiguredTempleTwoStartRoom()
    {
        GameObject mapObject = new GameObject("test map");
        mapObject.SetActive(false);

        try
        {
            Component map = mapObject.AddComponent(RuntimeType("MapEngine"));
            FieldInfo levelDataField = RuntimeType("MapEngine").GetField("LevelData", InstanceMembers);
            Assert.That(levelDataField, Is.Not.Null);
            levelDataField.SetValue(map, Resources.Load<TextAsset>("Levels/temple2"));

            Assert.That(levelDataField.GetValue(map), Is.Not.Null, "Temple 2 level data must be present in Resources.");
            Invoke(map, "LoadLevel");

            IDictionary levelInfo = (IDictionary)GetField<object>(map, "levelInfo");
            int x = int.Parse((string)levelInfo["StartRoomX"]);
            int y = int.Parse((string)levelInfo["StartRoomY"]);
            string startRoom = (string)levelInfo["StartRoom"];

            Invoke(map, "CreateRoom", x, y, startRoom);

            Assert.That((string)Invoke(map, "RoomAt", (float)x, (float)y), Is.EqualTo(startRoom));
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(mapObject);
        }
    }

    /// <summary>Resolves a gameplay component type from the project's predefined Assembly-CSharp assembly.</summary>
    private static Type RuntimeType(string typeName)
    {
        Type type = Type.GetType(typeName + ", Assembly-CSharp");
        Assert.That(type, Is.Not.Null, "Could not locate gameplay type " + typeName + " in Assembly-CSharp.");
        return type;
    }

    /// <summary>Invokes a public or non-public instance method on a gameplay component.</summary>
    private static object Invoke(object target, string methodName, params object[] arguments)
    {
        MethodInfo method = target.GetType().GetMethod(methodName, InstanceMembers);
        Assert.That(method, Is.Not.Null, "Could not locate method " + target.GetType().Name + "." + methodName + ".");
        return method.Invoke(target, arguments);
    }

    /// <summary>Sets a public or non-public instance field used by the test setup.</summary>
    private static void SetField(object target, string fieldName, object value)
    {
        FieldInfo field = target.GetType().GetField(fieldName, InstanceMembers);
        Assert.That(field, Is.Not.Null, "Could not locate field " + target.GetType().Name + "." + fieldName + ".");
        field.SetValue(target, value);
    }

    /// <summary>Reads a public or non-public instance field for an assertion.</summary>
    private static T GetField<T>(object target, string fieldName)
    {
        FieldInfo field = target.GetType().GetField(fieldName, InstanceMembers);
        Assert.That(field, Is.Not.Null, "Could not locate field " + target.GetType().Name + "." + fieldName + ".");
        return (T)field.GetValue(target);
    }

    /// <summary>
    /// Builds the scene objects expected by the colored-switch scripts and calls their initialization methods.
    /// Each switch has a SpriteRenderer and BoxCollider2D; the fixture also creates trap1 and a Player collider.
    /// It destroys all created objects when disposed so tests do not share state.
    /// </summary>
    private sealed class ColoredSwitchFixture : IDisposable
    {
        private readonly GameObject[] objects;
        private readonly Collider2D playerCollider;

        public readonly Component Red;
        public readonly Component Green;
        public readonly Component Blue;

        public ColoredSwitchFixture()
        {
            Type redType = RuntimeType("RedSwitchScript");
            Type greenType = RuntimeType("GreenSwitchScript");
            Type blueType = RuntimeType("BlueSwitchScript");
            Type trapType = RuntimeType("TrapSwitchT2Script");

            GameObject redObject = CreateSwitchObject("switchR", redType);
            GameObject greenObject = CreateSwitchObject("switchG", greenType);
            GameObject blueObject = CreateSwitchObject("switchB", blueType);
            GameObject trapObject = new GameObject("trap1");
            trapObject.AddComponent(trapType);
            GameObject playerObject = new GameObject("Player");
            playerCollider = playerObject.AddComponent<BoxCollider2D>();

            objects = new[] { redObject, greenObject, blueObject, trapObject, playerObject };
            Red = redObject.GetComponent(redType);
            Green = greenObject.GetComponent(greenType);
            Blue = blueObject.GetComponent(blueType);

            Invoke(Red, "Start");
            Invoke(Green, "Start");
            Invoke(Blue, "Start");
        }

        /// <summary>Simulates the gameplay trigger-stay callback for the test Player on one switch.</summary>
        public void Press(Component switchComponent)
        {
            Invoke(switchComponent, "OnTriggerStay2D", playerCollider);
        }

        /// <summary>Returns the switch's current triggered state for assertions.</summary>
        public bool Triggered(Component switchComponent)
        {
            return GetField<bool>(switchComponent, "triggered");
        }

        public void Dispose()
        {
            foreach (GameObject gameObject in objects)
                UnityEngine.Object.DestroyImmediate(gameObject);
        }

        private static GameObject CreateSwitchObject(string name, Type scriptType)
        {
            GameObject gameObject = new GameObject(name);
            gameObject.AddComponent<SpriteRenderer>();
            gameObject.AddComponent<BoxCollider2D>();
            gameObject.AddComponent(scriptType);
            return gameObject;
        }
    }
}
