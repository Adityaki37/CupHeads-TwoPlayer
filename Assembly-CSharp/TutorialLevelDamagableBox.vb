Imports System
Imports UnityEngine

' Token: 0x02000832 RID: 2098
Public Class TutorialLevelDamagableBox
	Inherits AbstractCollidableObject

	' Token: 0x060030AB RID: 12459 RVA: 0x001CA1BE File Offset: 0x001C85BE
	Private Sub Start()
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x060030AC RID: 12460 RVA: 0x001CA1D7 File Offset: 0x001C85D7
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Gizmos.color = Color.red
		Gizmos.DrawWireSphere(Me.explosionPosition + MyBase.transform.position, 10F)
	End Sub

	' Token: 0x060030AD RID: 12461 RVA: 0x001CA20C File Offset: 0x001C860C
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.boxHealth -= info.damage
		If Me.boxHealth <= 0F Then
			Me.toEnable.SetActive(True)
			Me.toDisable.SetActive(False)
			Me.explosionPrefab.Create(Me.explosionPosition + MyBase.transform.position)
			AudioManager.Play("sfx_object_explode")
			Me.emitAudioFromObject.Add("sfx_object_explode")
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x0400394D RID: 14669
	<SerializeField()>
	Private boxHealth As Single = 20F

	' Token: 0x0400394E RID: 14670
	<SerializeField()>
	Private toDisable As GameObject

	' Token: 0x0400394F RID: 14671
	<SerializeField()>
	Private toEnable As GameObject

	' Token: 0x04003950 RID: 14672
	<SerializeField()>
	Private explosionPrefab As PlatformingLevelGenericExplosion

	' Token: 0x04003951 RID: 14673
	<SerializeField()>
	Private explosionPosition As Vector3
End Class
