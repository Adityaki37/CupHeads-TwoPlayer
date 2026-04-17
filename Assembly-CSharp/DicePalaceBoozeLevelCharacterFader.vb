Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200059F RID: 1439
Public Class DicePalaceBoozeLevelCharacterFader
	Inherits AbstractPausableComponent

	' Token: 0x06001B9D RID: 7069 RVA: 0x000FB8C6 File Offset: 0x000F9CC6
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.mainSprite = MyBase.GetComponent(Of SpriteRenderer)()
		MyBase.StartCoroutine(Me.main_cr())
	End Sub

	' Token: 0x06001B9E RID: 7070 RVA: 0x000FB8E8 File Offset: 0x000F9CE8
	Private Iterator Function main_cr() As IEnumerator
		For Each spriteRenderer As SpriteRenderer In Me.sprites
			spriteRenderer.color = New Color(1F, 1F, 1F, 0F)
		Next
		Me.mainSprite.color = New Color(1F, 1F, 1F, 0F)
		Dim fadeTime As Single = 0.5F
		While True
			Dim holdTime As Single = Global.UnityEngine.Random.Range(3F, 6F)
			Yield CupheadTime.WaitForSeconds(Me, holdTime)
			If Me.fadingOut Then
				Dim t As Single = 0F
				While t < fadeTime
					Me.mainSprite.color = New Color(1F, 1F, 1F, 1F - t / fadeTime)
					For Each spriteRenderer2 As SpriteRenderer In Me.sprites
						spriteRenderer2.color = New Color(1F, 1F, 1F, 1F - t / fadeTime)
					Next
					t += CupheadTime.Delta
					Yield Nothing
				End While
			Else
				Dim t2 As Single = 0F
				While t2 < fadeTime
					Me.mainSprite.color = New Color(1F, 1F, 1F, t2 / fadeTime)
					For Each spriteRenderer3 As SpriteRenderer In Me.sprites
						spriteRenderer3.color = New Color(1F, 1F, 1F, t2 / fadeTime)
					Next
					t2 += CupheadTime.Delta
					Yield Nothing
				End While
			End If
			Me.fadingOut = Not Me.fadingOut
		End While
		Return
	End Function

	' Token: 0x040024AE RID: 9390
	<SerializeField()>
	Private sprites As SpriteRenderer()

	' Token: 0x040024AF RID: 9391
	Private mainSprite As SpriteRenderer

	' Token: 0x040024B0 RID: 9392
	Private fadingOut As Boolean
End Class
