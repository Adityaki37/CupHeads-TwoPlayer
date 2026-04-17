Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200068F RID: 1679
Public Class FlyingMermaidLevelMerdusaBodyPart
	Inherits LevelProperties.FlyingMermaid.Entity

	' Token: 0x170003A1 RID: 929
	' (get) Token: 0x0600236F RID: 9071 RVA: 0x0014C8AB File Offset: 0x0014ACAB
	' (set) Token: 0x06002370 RID: 9072 RVA: 0x0014C8B3 File Offset: 0x0014ACB3
	Public Property IsSinking As Boolean

	' Token: 0x06002371 RID: 9073 RVA: 0x0014C8BC File Offset: 0x0014ACBC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If Me.damagePlayer Then
			Me.damageDealer = DamageDealer.NewEnemy()
		End If
		MyBase.StartCoroutine(Me.main_cr())
		MyBase.StartCoroutine(Me.check_to_delete_cr())
	End Sub

	' Token: 0x06002372 RID: 9074 RVA: 0x0014C8F4 File Offset: 0x0014ACF4
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002373 RID: 9075 RVA: 0x0014C91D File Offset: 0x0014AD1D
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002374 RID: 9076 RVA: 0x0014C938 File Offset: 0x0014AD38
	Private Iterator Function main_cr() As IEnumerator
		AudioManager.Play("level_mermaid_merdusa_fallapart_break")
		Yield CupheadTime.WaitForSeconds(Me, Me.waitTime)
		If Me.stopBobbingAfterWait Then
			MyBase.GetComponent(Of FlyingMermaidLevelFloater)().enabled = False
		End If
		Me.IsSinking = True
		Dim t As Single = 0F
		While t < Me.moveTime
			MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.Delta, Me.velocity.y * CupheadTime.Delta, 0F)
			MyBase.transform.Rotate(0F, 0F, Me.rotationSpeed * CupheadTime.Delta)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002375 RID: 9077 RVA: 0x0014C954 File Offset: 0x0014AD54
	Public Function Create(pos As Vector2) As FlyingMermaidLevelMerdusaBodyPart
		Dim flyingMermaidLevelMerdusaBodyPart As FlyingMermaidLevelMerdusaBodyPart = Global.UnityEngine.[Object].Instantiate(Of FlyingMermaidLevelMerdusaBodyPart)(Me)
		flyingMermaidLevelMerdusaBodyPart.transform.SetPosition(New Single?(pos.x), New Single?(pos.y), Nothing)
		Return flyingMermaidLevelMerdusaBodyPart
	End Function

	' Token: 0x06002376 RID: 9078 RVA: 0x0014C998 File Offset: 0x0014AD98
	Private Iterator Function check_to_delete_cr() As IEnumerator
		While MyBase.transform.position.x >= -1140F AndAlso MyBase.transform.position.x <= 1140F AndAlso MyBase.transform.position.y >= -860F AndAlso MyBase.transform.position.y <= 1220F
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x04002C11 RID: 11281
	<SerializeField()>
	Private waitTime As Single

	' Token: 0x04002C12 RID: 11282
	<SerializeField()>
	Private velocity As Vector2

	' Token: 0x04002C13 RID: 11283
	<SerializeField()>
	Private moveTime As Single

	' Token: 0x04002C14 RID: 11284
	<SerializeField()>
	Private stopBobbingAfterWait As Boolean

	' Token: 0x04002C15 RID: 11285
	<SerializeField()>
	Private rotationSpeed As Single

	' Token: 0x04002C16 RID: 11286
	<SerializeField()>
	Private damagePlayer As Boolean

	' Token: 0x04002C17 RID: 11287
	Private damageDealer As DamageDealer
End Class
