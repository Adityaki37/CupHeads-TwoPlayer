Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007DB RID: 2011
Public Class SlimeLevelLightRay
	Inherits AbstractPausableComponent

	' Token: 0x06002DE7 RID: 11751 RVA: 0x001B0FDA File Offset: 0x001AF3DA
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.StartCoroutine(Me.main_cr())
	End Sub

	' Token: 0x06002DE8 RID: 11752 RVA: 0x001B0FF0 File Offset: 0x001AF3F0
	Private Iterator Function main_cr() As IEnumerator
		Dim fadingOut As Boolean = Me.startVisible
		Dim sprite As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		If Not Me.startVisible Then
			sprite.color = New Color(1F, 1F, 1F, 0F)
		End If
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.holdTime)
			If fadingOut Then
				Dim t As Single = 0F
				While t < Me.fadeTime
					sprite.color = New Color(1F, 1F, 1F, 1F - t / Me.fadeTime)
					t += CupheadTime.Delta
					Yield Nothing
				End While
				sprite.color = New Color(1F, 1F, 1F, 0F)
			Else
				Dim t2 As Single = 0F
				While t2 < Me.fadeTime
					sprite.color = New Color(1F, 1F, 1F, t2 / Me.fadeTime)
					t2 += CupheadTime.Delta
					Yield Nothing
				End While
				sprite.color = New Color(1F, 1F, 1F, 1F)
			End If
			fadingOut = Not fadingOut
		End While
		Return
	End Function

	' Token: 0x04003668 RID: 13928
	<SerializeField()>
	Private holdTime As Single

	' Token: 0x04003669 RID: 13929
	<SerializeField()>
	Private fadeTime As Single

	' Token: 0x0400366A RID: 13930
	<SerializeField()>
	Private startVisible As Boolean
End Class
