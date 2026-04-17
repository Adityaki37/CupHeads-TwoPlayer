Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020006AF RID: 1711
Public Class FrogsLevelMorphedSlot
	Inherits AbstractPausableComponent

	' Token: 0x170003AE RID: 942
	' (get) Token: 0x06002451 RID: 9297 RVA: 0x001552B0 File Offset: 0x001536B0
	' (set) Token: 0x06002452 RID: 9298 RVA: 0x001552B8 File Offset: 0x001536B8
	Public Property mode As Slots.Mode

	' Token: 0x170003AF RID: 943
	' (get) Token: 0x06002453 RID: 9299 RVA: 0x001552C1 File Offset: 0x001536C1
	' (set) Token: 0x06002454 RID: 9300 RVA: 0x001552C9 File Offset: 0x001536C9
	Public Property state As FrogsLevelMorphedSlot.State

	' Token: 0x170003B0 RID: 944
	' (get) Token: 0x06002455 RID: 9301 RVA: 0x001552D2 File Offset: 0x001536D2
	' (set) Token: 0x06002456 RID: 9302 RVA: 0x001552DA File Offset: 0x001536DA
	Public Property action As FrogsLevelMorphedSlot.Action

	' Token: 0x06002457 RID: 9303 RVA: 0x001552E4 File Offset: 0x001536E4
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.offsets = New Dictionary(Of Slots.Mode, Single)()
		Me.offsets(Slots.Mode.Snake) = 0.4F
		Me.offsets(Slots.Mode.Tiger) = -0.095F
		Me.offsets(Slots.Mode.Bison) = 0.095F
		Me.offsets(Slots.Mode.Oni) = -0.4F
		Me.mat = MyBase.GetComponent(Of Renderer)().material
		Me.SetTexture(Me.textures.[Get](FrogsLevelMorphedSlot.State.Normal, 0))
		Me.SetOffset(Me.offsets(Slots.Mode.Snake))
	End Sub

	' Token: 0x06002458 RID: 9304 RVA: 0x0015537C File Offset: 0x0015377C
	Private Sub Start()
		MyBase.StartCoroutine(Me.animate_cr())
	End Sub

	' Token: 0x06002459 RID: 9305 RVA: 0x0015538B File Offset: 0x0015378B
	Private Sub SetTexture(texture As Texture2D)
		Me.mat.mainTexture = texture
	End Sub

	' Token: 0x0600245A RID: 9306 RVA: 0x0015539C File Offset: 0x0015379C
	Private Sub SetOffset(y As Single)
		Dim mainTextureOffset As Vector2 = Me.mat.mainTextureOffset
		mainTextureOffset.y = y
		Me.mat.mainTextureOffset = mainTextureOffset
	End Sub

	' Token: 0x0600245B RID: 9307 RVA: 0x001553C9 File Offset: 0x001537C9
	Public Sub StartSpin()
		AudioManager.PlayLoop("level_frogs_morphed_spin_loop")
		Me.emitAudioFromObject.Add("level_frogs_morphed_spin_loop")
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.animate_cr())
		MyBase.StartCoroutine(Me.spin_cr())
	End Sub

	' Token: 0x0600245C RID: 9308 RVA: 0x00155408 File Offset: 0x00153808
	Public Sub StopSpin(mode As Slots.Mode)
		AudioManager.[Stop]("level_frogs_morphed_spin_loop")
		Me.emitAudioFromObject.Add("level_frogs_morphed_spin_loop")
		AudioManager.Play("level_frogs_morphed_spin")
		Me.emitAudioFromObject.Add("level_frogs_morphed_spin")
		Me.mode = mode
		Me.action = FrogsLevelMorphedSlot.Action.Ending
	End Sub

	' Token: 0x0600245D RID: 9309 RVA: 0x00155457 File Offset: 0x00153857
	Public Sub Flash()
		MyBase.StartCoroutine(Me.flash_cr())
	End Sub

	' Token: 0x0600245E RID: 9310 RVA: 0x00155468 File Offset: 0x00153868
	Private Iterator Function animate_cr() As IEnumerator
		Dim frame As Integer = 0
		While True
			Yield CupheadTime.WaitForSeconds(Me, 0.06F)
			Me.SetTexture(Me.textures.[Get](Me.state, frame))
			frame = CInt(Mathf.Repeat(CSng((frame + 1)), 3F))
		End While
		Return
	End Function

	' Token: 0x0600245F RID: 9311 RVA: 0x00155484 File Offset: 0x00153884
	Private Iterator Function spin_cr() As IEnumerator
		Dim offset As Single = Me.mat.mainTextureOffset.y
		Me.action = FrogsLevelMorphedSlot.Action.Spinning
		While Me.action = FrogsLevelMorphedSlot.Action.Spinning
			offset = Mathf.Repeat(offset + 5F * CupheadTime.Delta, 1F)
			Me.SetOffset(offset)
			Yield Nothing
		End While
		Dim t As Single = 0F
		Me.SetOffset(-3F)
		Dim startOffset As Single = Me.mat.mainTextureOffset.y
		While t < 1F
			Dim val As Single = t / 1F
			Dim o As Single = EaseUtils.Ease(EaseUtils.EaseType.easeOutElastic, startOffset, Me.offsets(Me.mode), val)
			Me.SetOffset(o)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002460 RID: 9312 RVA: 0x001554A0 File Offset: 0x001538A0
	Private Iterator Function flash_cr() As IEnumerator
		Me.state = FrogsLevelMorphedSlot.State.Flashing
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		Me.state = FrogsLevelMorphedSlot.State.Normal
		Return
	End Function

	' Token: 0x06002461 RID: 9313 RVA: 0x001554BB File Offset: 0x001538BB
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Global.UnityEngine.[Object].Destroy(Me.mat)
		Me.textures.flashing = Nothing
		Me.textures.normal = Nothing
	End Sub

	' Token: 0x04002D0C RID: 11532
	Private Const STOP_OFFSET As Single = 3F

	' Token: 0x04002D0D RID: 11533
	Private Const STOP_TIME As Single = 1F

	' Token: 0x04002D0E RID: 11534
	Private Const OFFSET_SPEED As Single = 5F

	' Token: 0x04002D0F RID: 11535
	Private Const FLASH_TIME As Single = 0.2F

	' Token: 0x04002D10 RID: 11536
	<SerializeField()>
	Private textures As FrogsLevelMorphedSlot.Textures

	' Token: 0x04002D14 RID: 11540
	Private mat As Material

	' Token: 0x04002D15 RID: 11541
	Private offsets As Dictionary(Of Slots.Mode, Single)

	' Token: 0x020006B0 RID: 1712
	Public Enum State
		' Token: 0x04002D17 RID: 11543
		Normal
		' Token: 0x04002D18 RID: 11544
		Flashing
	End Enum

	' Token: 0x020006B1 RID: 1713
	Public Enum Action
		' Token: 0x04002D1A RID: 11546
		[Static]
		' Token: 0x04002D1B RID: 11547
		Spinning
		' Token: 0x04002D1C RID: 11548
		Ending
	End Enum

	' Token: 0x020006B2 RID: 1714
	<Serializable()>
	Public Class Textures
		' Token: 0x06002463 RID: 9315 RVA: 0x001554EE File Offset: 0x001538EE
		Public Function [Get](state As FrogsLevelMorphedSlot.State, frame As Integer) As Texture2D
			If state = FrogsLevelMorphedSlot.State.Normal OrElse state <> FrogsLevelMorphedSlot.State.Flashing Then
				Return Me.normal(frame)
			End If
			Return Me.flashing(frame)
		End Function

		' Token: 0x04002D1D RID: 11549
		Public normal As Texture2D()

		' Token: 0x04002D1E RID: 11550
		Public flashing As Texture2D()
	End Class
End Class
