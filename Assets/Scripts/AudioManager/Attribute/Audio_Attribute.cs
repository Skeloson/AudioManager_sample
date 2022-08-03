using UnityEngine;

public class Audio_Attribute : PropertyAttribute
{
    // -- PROPERTIES

    public string[] Names
    {
        get;
        private set;
    }

    // -- METHODS

    public Audio_Attribute()
    {
#if UNITY_EDITOR
        string[] l_items = UnityEditor.AssetDatabase.FindAssets( "t:Audio_Manager" );
        if( l_items != null && l_items.Length > 0 )
        {
            Audio_Manager l_manager = UnityEditor.AssetDatabase.LoadAssetAtPath<Audio_Manager>( UnityEditor.AssetDatabase.GUIDToAssetPath( l_items[0] ) );
            Names = l_manager.GetIDs();
        }
        else
        {
            Names = new string[] { "___" };
        }
#endif
    }
}