Imports System
Imports System.Collections.Generic
Imports TMPro
Imports UnityEngine

' Token: 0x02000917 RID: 2327
Public Class FontReplacer
	Inherits MonoBehaviour

	' Token: 0x06003677 RID: 13943 RVA: 0x001F87F7 File Offset: 0x001F6BF7
	Private Sub Awake()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04003E70 RID: 15984
	Public localizationAsset As Localization

	' Token: 0x04003E71 RID: 15985
	Public sourceLanguage As Localization.Languages

	' Token: 0x04003E72 RID: 15986
	Public destinationLanguage As Localization.Languages

	' Token: 0x04003E73 RID: 15987
	Public allSourceFonts As List(Of Font)

	' Token: 0x04003E74 RID: 15988
	Public allDestinationFonts As List(Of Font)

	' Token: 0x04003E75 RID: 15989
	Public allSourceFontAssets As List(Of TMP_FontAsset)

	' Token: 0x04003E76 RID: 15990
	Public allDestinationFontAssets As List(Of TMP_FontAsset)
End Class
