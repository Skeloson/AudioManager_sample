using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( Audio_Manager ) )]
public class Audio_Manager_Editor : Editor
{
    SerializedProperty m_soundBank;
    string SearchText = "";

    protected void OnEnable()
    {
        m_soundBank = serializedObject.FindProperty( "SoundBank" );
    }

    public override void OnInspectorGUI()
    {
        SearchText = GUILayout.TextField( SearchText, GUI.skin.FindStyle( "ToolbarSeachTextField" ) );

        if( string.IsNullOrEmpty( SearchText ) )
        {
            EditorGUILayout.PropertyField( m_soundBank, true );
        }
        else
        {
            for( int bankindex = 0; bankindex < m_soundBank.arraySize; bankindex++ )
            {
                SerializedProperty l_soundArray = m_soundBank.GetArrayElementAtIndex( bankindex ).FindPropertyRelative( "SoundArray" );

                for( int soundIndex = 0; soundIndex < l_soundArray.arraySize; soundIndex++ )
                {
                    if( l_soundArray.GetArrayElementAtIndex( soundIndex ).FindPropertyRelative( "MyName" ).stringValue.ToLower().Contains( SearchText.ToLower() ) )
                    {
                        EditorGUILayout.PropertyField( m_soundBank.GetArrayElementAtIndex( bankindex ).FindPropertyRelative( "SoundArray" ).GetArrayElementAtIndex( soundIndex ), true );
                    }
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
