Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000941 RID: 2369
Public Class MapLevelMausoleumEntity
	Inherits AbstractMapLevelDependentEntity

	' Token: 0x06003766 RID: 14182 RVA: 0x001FDC7C File Offset: 0x001FC07C
	Public Overrides Sub OnConditionNotMet()
		If Me.ToEnable IsNot Nothing Then
			Me.ToEnable.SetActive(False)
		End If
		If Me.ToDisable IsNot Nothing Then
			Me.ToDisable.SetActive(True)
		End If
	End Sub

	' Token: 0x06003767 RID: 14183 RVA: 0x001FDCB8 File Offset: 0x001FC0B8
	Public Overrides Sub OnConditionMet()
		If Me.ToEnable IsNot Nothing Then
			Me.ToEnable.SetActive(False)
		End If
		If Me.ToDisable IsNot Nothing Then
			Me.ToDisable.SetActive(True)
		End If
	End Sub

	' Token: 0x06003768 RID: 14184 RVA: 0x001FDCF4 File Offset: 0x001FC0F4
	Public Overrides Sub OnConditionAlreadyMet()
		If Me.ToEnable IsNot Nothing Then
			Me.ToEnable.SetActive(True)
		End If
		If Me.ToDisable IsNot Nothing Then
			Me.ToDisable.SetActive(False)
		End If
	End Sub

	' Token: 0x06003769 RID: 14185 RVA: 0x001FDD30 File Offset: 0x001FC130
	Protected Overrides Function ValidateSucess() As Boolean
		Return PlayerData.Data.IsUnlocked(PlayerId.PlayerOne, Me.superUnlock) AndAlso PlayerData.Data.IsUnlocked(PlayerId.PlayerTwo, Me.superUnlock)
	End Function

	' Token: 0x0600376A RID: 14186 RVA: 0x001FDD5C File Offset: 0x001FC15C
	Protected Overrides Function ValidateCondition(level As Levels) As Boolean
		Return Level.Won AndAlso Level.PreviousLevel = level AndAlso Level.SuperUnlocked
	End Function

	' Token: 0x0600376B RID: 14187 RVA: 0x001FDD84 File Offset: 0x001FC184
	Public Overrides Sub DoTransition()
		MyBase.StartCoroutine(Me.transition_cr())
	End Sub

	' Token: 0x0600376C RID: 14188 RVA: 0x001FDD94 File Offset: 0x001FC194
	Private Iterator Function transition_cr() As IEnumerator
		Me.poofPrefab.Create(Me.poofRoot.position, New Vector3(0.01F, 0.01F, 1F))
		AudioManager.Play("world_map_mausoleum_destruction")
		Yield CupheadTime.WaitForSeconds(Me, 0.25F)
		Me.ToEnable.SetActive(True)
		Me.ToDisable.SetActive(False)
		Yield CupheadTime.WaitForSeconds(Me, 0.36F)
		MyBase.CurrentState = AbstractMapLevelDependentEntity.State.Complete
		Return
	End Function

	' Token: 0x04003F7B RID: 16251
	<SerializeField()>
	Private ToEnable As GameObject

	' Token: 0x04003F7C RID: 16252
	<SerializeField()>
	Private ToDisable As GameObject

	' Token: 0x04003F7D RID: 16253
	<SerializeField()>
	Private poofPrefab As Effect

	' Token: 0x04003F7E RID: 16254
	<SerializeField()>
	Private poofRoot As Transform

	' Token: 0x04003F7F RID: 16255
	<SerializeField()>
	Private superUnlock As Super
End Class
