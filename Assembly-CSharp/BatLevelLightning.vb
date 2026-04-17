Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000508 RID: 1288
Public Class BatLevelLightning
	Inherits AbstractCollidableObject

	' Token: 0x060016D7 RID: 5847 RVA: 0x000CD684 File Offset: 0x000CBA84
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.collisionChild = Me.lightning.GetComponent(Of CollisionChild)()
		AddHandler Me.collisionChild.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		Me.lightning.SetActive(False)
	End Sub

	' Token: 0x060016D8 RID: 5848 RVA: 0x000CD6D7 File Offset: 0x000CBAD7
	Public Sub Init(properties As LevelProperties.Bat.BatLightning, startPos As Vector2)
		Me.properties = properties
		MyBase.transform.position = startPos
		MyBase.StartCoroutine(Me.lightning_cr())
	End Sub

	' Token: 0x060016D9 RID: 5849 RVA: 0x000CD700 File Offset: 0x000CBB00
	Private Iterator Function lightning_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.cloudWarning)
		Me.lightning.SetActive(True)
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.lightningOnDuration)
		Me.Die()
		Return
	End Function

	' Token: 0x060016DA RID: 5850 RVA: 0x000CD71B File Offset: 0x000CBB1B
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x060016DB RID: 5851 RVA: 0x000CD732 File Offset: 0x000CBB32
	Private Sub Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04002024 RID: 8228
	<SerializeField()>
	Private lightning As GameObject

	' Token: 0x04002025 RID: 8229
	Private collisionChild As CollisionChild

	' Token: 0x04002026 RID: 8230
	Private properties As LevelProperties.Bat.BatLightning

	' Token: 0x04002027 RID: 8231
	Private damageDealer As DamageDealer
End Class
