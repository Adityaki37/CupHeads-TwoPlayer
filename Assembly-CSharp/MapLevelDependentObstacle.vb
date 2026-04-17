Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200093C RID: 2364
Public Class MapLevelDependentObstacle
	Inherits AbstractMapLevelDependentEntity

	' Token: 0x06003751 RID: 14161 RVA: 0x001FA8B7 File Offset: 0x001F8CB7
	Protected Overrides Sub Start()
		MyBase.Start()
	End Sub

	' Token: 0x06003752 RID: 14162 RVA: 0x001FA8BF File Offset: 0x001F8CBF
	Public Overrides Sub OnConditionNotMet()
		If Me.ToEnable IsNot Nothing Then
			Me.ToEnable.SetActive(False)
		End If
		If Me.ToDisable IsNot Nothing Then
			Me.ToDisable.SetActive(True)
		End If
	End Sub

	' Token: 0x06003753 RID: 14163 RVA: 0x001FA8FB File Offset: 0x001F8CFB
	Public Overrides Sub OnConditionMet()
		If Me.ToEnable IsNot Nothing Then
			Me.ToEnable.SetActive(False)
		End If
		If Me.ToDisable IsNot Nothing Then
			Me.ToDisable.SetActive(True)
		End If
	End Sub

	' Token: 0x06003754 RID: 14164 RVA: 0x001FA937 File Offset: 0x001F8D37
	Public Overrides Sub OnConditionAlreadyMet()
		If Me.ToEnable IsNot Nothing Then
			Me.ToEnable.SetActive(True)
		End If
		If Me.ToDisable IsNot Nothing Then
			Me.ToDisable.SetActive(False)
		End If
	End Sub

	' Token: 0x06003755 RID: 14165 RVA: 0x001FA973 File Offset: 0x001F8D73
	Public Overrides Sub DoTransition()
		MyBase.StartCoroutine(Me.transition_cr())
	End Sub

	' Token: 0x06003756 RID: 14166 RVA: 0x001FA982 File Offset: 0x001F8D82
	Public Sub OnChange()
		If Me.ToEnable IsNot Nothing Then
			Me.ToEnable.SetActive(False)
		End If
		If Me.ToDisable IsNot Nothing Then
			Me.ToDisable.SetActive(True)
		End If
	End Sub

	' Token: 0x06003757 RID: 14167 RVA: 0x001FA9C0 File Offset: 0x001F8DC0
	Private Iterator Function transition_cr() As IEnumerator
		AudioManager.Play("world_level_bridge_building_poof")
		Me.poofPrefab.Create(Me.poofRoot.position, New Vector3(0.01F, 0.01F, 1F))
		Yield CupheadTime.WaitForSeconds(Me, 0.25F)
		Dim sprites As SpriteRenderer() = MyBase.GetComponentsInChildren(Of SpriteRenderer)(True)
		For Each spriteRenderer As SpriteRenderer In sprites
			spriteRenderer.material = Me.flashMaterial
		Next
		If Not Me.seeDisabledOnlyDuringTransition Then
			Me.ToEnable.SetActive(True)
		End If
		If Me.seeEnableOnlyDuringTransition Then
			Me.ToDisable.SetActive(False)
		End If
		Yield CupheadTime.WaitForSeconds(Me, 0.04F)
		For i As Integer = 0 To 4 - 1
			For Each spriteRenderer2 As SpriteRenderer In sprites
				spriteRenderer2.color = Color.white
			Next
			Yield CupheadTime.WaitForSeconds(Me, 0.04F)
			For Each spriteRenderer3 As SpriteRenderer In sprites
				spriteRenderer3.color = Color.black
			Next
			Yield CupheadTime.WaitForSeconds(Me, 0.04F)
		Next
		If Me.seeDisabledOnlyDuringTransition Then
			Me.ToEnable.SetActive(True)
		End If
		If Not Me.seeEnableOnlyDuringTransition AndAlso Me.ToDisable IsNot Nothing Then
			Me.ToDisable.SetActive(False)
		End If
		MyBase.CurrentState = AbstractMapLevelDependentEntity.State.Complete
		Return
	End Function

	' Token: 0x04003F6B RID: 16235
	<SerializeField()>
	Private ToEnable As GameObject

	' Token: 0x04003F6C RID: 16236
	<SerializeField()>
	Private seeEnableOnlyDuringTransition As Boolean

	' Token: 0x04003F6D RID: 16237
	<SerializeField()>
	Private ToDisable As GameObject

	' Token: 0x04003F6E RID: 16238
	<SerializeField()>
	Private seeDisabledOnlyDuringTransition As Boolean

	' Token: 0x04003F6F RID: 16239
	<SerializeField()>
	Private poofPrefab As Effect

	' Token: 0x04003F70 RID: 16240
	<SerializeField()>
	Private flashMaterial As Material

	' Token: 0x04003F71 RID: 16241
	<SerializeField()>
	Private poofRoot As Transform

	' Token: 0x04003F72 RID: 16242
	<SerializeField()>
	Private DontPlayPoofSFX As Boolean
End Class
