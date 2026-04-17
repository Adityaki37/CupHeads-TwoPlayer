Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000663 RID: 1635
Public Class FlyingGenieLevelBomb
	Inherits AbstractProjectile

	' Token: 0x0600220C RID: 8716 RVA: 0x0013D29C File Offset: 0x0013B69C
	Public Function Create(pos As Vector2, targetPos As Vector3, properties As LevelProperties.FlyingGenie.Bomb) As FlyingGenieLevelBomb
		Dim flyingGenieLevelBomb As FlyingGenieLevelBomb = TryCast(MyBase.Create(), FlyingGenieLevelBomb)
		flyingGenieLevelBomb.transform.position = pos
		flyingGenieLevelBomb.properties = properties
		flyingGenieLevelBomb.targetPos = targetPos
		Return flyingGenieLevelBomb
	End Function

	' Token: 0x0600220D RID: 8717 RVA: 0x0013D2D8 File Offset: 0x0013B6D8
	Protected Overrides Sub Awake()
		MyBase.Awake()
		For Each gameObject As GameObject In Me.explosionBeams
			gameObject.GetComponent(Of SpriteRenderer)().enabled = False
			gameObject.GetComponent(Of Collider2D)().enabled = False
			AddHandler gameObject.GetComponent(Of CollisionChild)().OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		Next
	End Sub

	' Token: 0x0600220E RID: 8718 RVA: 0x0013D33A File Offset: 0x0013B73A
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600220F RID: 8719 RVA: 0x0013D358 File Offset: 0x0013B758
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002210 RID: 8720 RVA: 0x0013D378 File Offset: 0x0013B778
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.readyToDetonate = False
		For Each gameObject As GameObject In Me.explosionBeams
			If Me.bombType = FlyingGenieLevelBomb.BombType.Regular Then
				gameObject.transform.SetScale(New Single?(Me.properties.bombRegularSize), New Single?(Me.properties.bombRegularSize), Nothing)
			ElseIf Me.bombType = FlyingGenieLevelBomb.BombType.Diagonal Then
				gameObject.transform.SetScale(New Single?(Me.properties.bombDiagonalSize), New Single?(Me.properties.bombDiagonalSize), Nothing)
			ElseIf Me.bombType = FlyingGenieLevelBomb.BombType.PlusSized Then
				gameObject.transform.SetScale(New Single?(Me.properties.bombPlusSize), New Single?(Me.properties.bombPlusSize), Nothing)
			End If
		Next
		MyBase.StartCoroutine(Me.start_cr())
	End Sub

	' Token: 0x06002211 RID: 8721 RVA: 0x0013D488 File Offset: 0x0013B888
	Private Iterator Function start_cr() As IEnumerator
		While MyBase.transform.position <> Me.targetPos
			MyBase.transform.position = Vector3.MoveTowards(MyBase.transform.position, Me.targetPos, Me.properties.bombSpeed * CupheadTime.Delta)
			Yield Nothing
		End While
		Me.readyToDetonate = True
		Yield Nothing
		Return
	End Function

	' Token: 0x06002212 RID: 8722 RVA: 0x0013D4A3 File Offset: 0x0013B8A3
	Public Sub Explode()
		MyBase.StartCoroutine(Me.explode_cr())
	End Sub

	' Token: 0x06002213 RID: 8723 RVA: 0x0013D4B4 File Offset: 0x0013B8B4
	Private Iterator Function explode_cr() As IEnumerator
		For Each gameObject As GameObject In Me.explosionBeams
			gameObject.GetComponent(Of SpriteRenderer)().enabled = True
			gameObject.GetComponent(Of Collider2D)().enabled = True
		Next
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		For Each gameObject2 As GameObject In Me.explosionBeams
			gameObject2.gameObject.SetActive(False)
		Next
		Me.readyToDetonate = False
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002214 RID: 8724 RVA: 0x0013D4CF File Offset: 0x0013B8CF
	Protected Overrides Sub Die()
		Me.StopAllCoroutines()
		MyBase.Die()
	End Sub

	' Token: 0x04002ABA RID: 10938
	Public bombType As FlyingGenieLevelBomb.BombType

	' Token: 0x04002ABB RID: 10939
	Public readyToDetonate As Boolean

	' Token: 0x04002ABC RID: 10940
	<SerializeField()>
	Private explosionBeams As GameObject()

	' Token: 0x04002ABD RID: 10941
	Private properties As LevelProperties.FlyingGenie.Bomb

	' Token: 0x04002ABE RID: 10942
	Private targetPos As Vector3

	' Token: 0x02000664 RID: 1636
	Public Enum BombType
		' Token: 0x04002AC0 RID: 10944
		Regular
		' Token: 0x04002AC1 RID: 10945
		Diagonal
		' Token: 0x04002AC2 RID: 10946
		PlusSized
	End Enum
End Class
