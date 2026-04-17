Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000387 RID: 903
Public Class UIImageAnimationLoop
	Inherits AbstractMonoBehaviour

	' Token: 0x06000AB4 RID: 2740 RVA: 0x0007FE73 File Offset: 0x0007E273
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.image = MyBase.GetComponent(Of Image)()
		Me.ignoreGlobalTime = Me.IgnoreGlobalTime
	End Sub

	' Token: 0x06000AB5 RID: 2741 RVA: 0x0007FE93 File Offset: 0x0007E293
	Private Sub Start()
		MyBase.StartCoroutine(Me.anim_cr())
	End Sub

	' Token: 0x06000AB6 RID: 2742 RVA: 0x0007FEA4 File Offset: 0x0007E2A4
	Private Sub Shuffle()
		Dim list As List(Of Sprite) = New List(Of Sprite)(Me.sprites)
		Dim random As Global.System.Random = New Global.System.Random()
		Dim i As Integer = list.Count
		While i > 1
			i -= 1
			Dim num As Integer = random.[Next](i + 1)
			Dim sprite As Sprite = list(num)
			list(num) = list(i)
			list(i) = sprite
		End While
		Me.sprites = list.ToArray()
	End Sub

	' Token: 0x06000AB7 RID: 2743 RVA: 0x0007FF10 File Offset: 0x0007E310
	Private Iterator Function anim_cr() As IEnumerator
		If Me.mode = UIImageAnimationLoop.Mode.Shuffle Then
			Me.Shuffle()
		End If
		Dim waitInstruction As YieldInstruction = New WaitForSeconds(Me.frameDelay)
		Dim i As Integer = 0
		While True
			Me.image.sprite = Me.sprites(i)
			i += 1
			If i >= Me.sprites.Length Then
				i = 0
			End If
			If Not Me.IgnoreGlobalTime Then
				Dim t As Single = 0F
				While t < Me.frameDelay
					t += CupheadTime.Delta(CupheadTime.Layer.[Default])
					Yield Nothing
				End While
			Else
				Yield waitInstruction
			End If
			If Me.mode = UIImageAnimationLoop.Mode.Random Then
				Me.Shuffle()
			End If
		End While
		Return
	End Function

	' Token: 0x06000AB8 RID: 2744 RVA: 0x0007FF2B File Offset: 0x0007E32B
	Private Sub OnDestroy()
		Me.sprites = Nothing
	End Sub

	' Token: 0x04001482 RID: 5250
	<SerializeField()>
	Private mode As UIImageAnimationLoop.Mode

	' Token: 0x04001483 RID: 5251
	<SerializeField()>
	Private frameDelay As Single = 0.07F

	' Token: 0x04001484 RID: 5252
	<SerializeField()>
	Private sprites As Sprite()

	' Token: 0x04001485 RID: 5253
	<SerializeField()>
	Private IgnoreGlobalTime As Boolean

	' Token: 0x04001486 RID: 5254
	Private image As Image

	' Token: 0x02000388 RID: 904
	Public Enum Mode
		' Token: 0x04001488 RID: 5256
		Linear
		' Token: 0x04001489 RID: 5257
		Shuffle
		' Token: 0x0400148A RID: 5258
		Random
	End Enum
End Class
