using System.Numerics;
using Elderforge.Core.Server.GameObjects;
using Elderforge.Shared.Types;


namespace Elderforge.Tests;

public class GameObjectsTests
{
    [Fact]
    public void Test_GameObjectPositionPropertyChanged()
    {
        var gameObject = new LightGameObject(LightType.Point);

        bool propertyChanged;

        gameObject.PositionChanged += (go, position) =>
        {
            propertyChanged = true;

            Assert.Equal(Vector3.One, position);

            Assert.True(propertyChanged);
        };

        gameObject.Position = Vector3.One;
    }

    [Fact]
    public void Test_GameObjectRotationPropertyChanged()
    {
        var gameObject = new LightGameObject(LightType.Point);

        bool propertyChanged;

        gameObject.RotationChanged += (go, rotation) =>
        {
            propertyChanged = true;

            Assert.Equal(Vector3.One, rotation);

            Assert.True(propertyChanged);
        };

        gameObject.Rotation = Vector3.One;
    }

    [Fact]
    public void Test_GameObjectScalePropertyChanged()
    {
        var gameObject = new LightGameObject(LightType.Directional);

        bool propertyChanged;

        gameObject.ScaleChanged += (go, scale) =>
        {
            propertyChanged = true;

            Assert.Equal(Vector3.One, scale);

            Assert.True(propertyChanged);
        };

        gameObject.Scale = Vector3.One;
    }
}
