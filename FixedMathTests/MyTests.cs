namespace FixMath.NET;

public class MyTests
{
    [TestCase(1, 2, 3)]
    [TestCase(10, 2, 12)]
    public void TestAdd(int a, int b, int expectedResult)
    {
        // Act
        var result = a + b;
        
        // Assert
        Assert.That(expectedResult, Is.EqualTo(result));
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    [Test]
    public void TestAddFix()
    {
        var a = (Fix64)1;
        var b = new Fix64(2);
        
        var result = a + b;
        
        Assert.That(result, Is.EqualTo(new Fix64(3)));
    }
}