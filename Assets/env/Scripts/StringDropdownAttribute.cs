using UnityEngine;
 
public class StringDropdownAttribute : PropertyAttribute
{
    public readonly string[] options;
    
    public StringDropdownAttribute(params string[] options)
    {
        this.options = options;
    }
} 
