Imports System
Imports Rewired.UI.ControlMapper
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000C29 RID: 3113
Public Class CustomButtonNavOverride
	Inherits CustomButton

	' Token: 0x06004C10 RID: 19472 RVA: 0x002721BD File Offset: 0x002705BD
	Public Overrides Function FindSelectableOnUp() As Selectable
		If Not PlayerManager.Multiplayer Then
			Return Me.upOnSinglePlayer
		End If
		Return If((Not Me.upOnMultiPlayer), Me.mapper.GetUnselectedPlayerButton(), Me.upOnMultiPlayer)
	End Function

	' Token: 0x06004C11 RID: 19473 RVA: 0x002721F8 File Offset: 0x002705F8
	Public Overrides Function FindSelectableOnDown() As Selectable
		If PlayerManager.Multiplayer Then
			Return If((Not Me.downOnMultiPlayer), Me.mapper.GetUnselectedPlayerButton(), Me.downOnMultiPlayer)
		End If
		If PlatformHelper.IsConsole Then
			Return Me
		End If
		Return Me.downOnSinglePlayer
	End Function

	' Token: 0x040050B5 RID: 20661
	<SerializeField()>
	Private upOnSinglePlayer As Selectable

	' Token: 0x040050B6 RID: 20662
	<SerializeField()>
	Private downOnSinglePlayer As Selectable

	' Token: 0x040050B7 RID: 20663
	<SerializeField()>
	Private upOnMultiPlayer As Selectable

	' Token: 0x040050B8 RID: 20664
	<SerializeField()>
	Private downOnMultiPlayer As Selectable

	' Token: 0x040050B9 RID: 20665
	<SerializeField()>
	Private mapper As ControlMapper
End Class
