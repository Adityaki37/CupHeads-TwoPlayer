Imports System
Imports UnityEngine

' Token: 0x0200049C RID: 1180
Public Class LevelAudio
	Inherits AbstractMonoBehaviour

	' Token: 0x06001338 RID: 4920 RVA: 0x000AA30C File Offset: 0x000A870C
	Public Shared Function Create() As LevelAudio
		Return Global.UnityEngine.[Object].Instantiate(Of LevelAudio)(Level.Current.LevelResources.levelAudio)
	End Function
End Class
