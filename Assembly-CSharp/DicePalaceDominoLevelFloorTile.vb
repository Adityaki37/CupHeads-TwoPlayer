Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005BB RID: 1467
Public Class DicePalaceDominoLevelFloorTile
	Inherits DicePalaceDominoLevelBaseTile

	' Token: 0x17000361 RID: 865
	' (get) Token: 0x06001C7D RID: 7293 RVA: 0x00104BB1 File Offset: 0x00102FB1
	' (set) Token: 0x06001C7E RID: 7294 RVA: 0x00104BB9 File Offset: 0x00102FB9
	Public Property spikesActive As Boolean

	' Token: 0x06001C7F RID: 7295 RVA: 0x00104BC2 File Offset: 0x00102FC2
	Protected Overrides Sub Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.boxCollider = MyBase.GetComponent(Of BoxCollider2D)()
		MyBase.Awake()
	End Sub

	' Token: 0x06001C80 RID: 7296 RVA: 0x00104BE1 File Offset: 0x00102FE1
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001C81 RID: 7297 RVA: 0x00104BF9 File Offset: 0x00102FF9
	Public Overrides Sub InitTile()
		MyBase.InitTile()
		Me.OnMoveStart()
	End Sub

	' Token: 0x06001C82 RID: 7298 RVA: 0x00104C07 File Offset: 0x00103007
	Public Sub SetColour(colourIndex As Integer, properties As LevelProperties.DicePalaceDomino)
		Me.properties = properties
		MyBase.currentColourIndex = colourIndex
		Me.spriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		Me.spriteRenderer.sprite = Me.colours(MyBase.currentColourIndex)
	End Sub

	' Token: 0x06001C83 RID: 7299 RVA: 0x00104C3B File Offset: 0x0010303B
	Public Sub OnMoveStart()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001C84 RID: 7300 RVA: 0x00104C4C File Offset: 0x0010304C
	Private Iterator Function move_cr() As IEnumerator
		Yield Nothing
		While MyBase.isActivated
			MyBase.transform.position += Vector3.left * Me.properties.CurrentState.domino.floorSpeed * CupheadTime.Delta
			If MyBase.transform.position.x + 200F < CSng(Level.Current.Left) Then
				Me.DeactivateTile()
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001C85 RID: 7301 RVA: 0x00104C67 File Offset: 0x00103067
	Public Sub TriggerSpikes(spikesActive As Boolean)
		MyBase.StartCoroutine(Me.toggleSpikes_cr(spikesActive))
	End Sub

	' Token: 0x06001C86 RID: 7302 RVA: 0x00104C77 File Offset: 0x00103077
	Public Overrides Sub DeactivateTile()
		MyBase.DeactivateTile()
		Me.toggleSpikes_cr(False)
	End Sub

	' Token: 0x06001C87 RID: 7303 RVA: 0x00104C88 File Offset: 0x00103088
	Private Iterator Function toggleSpikes_cr(spikesActive As Boolean) As IEnumerator
		If spikesActive Then
			MyBase.animator.Play("Spikes_Up")
			Me.spikesActive = True
			Me.boxCollider.enabled = True
		Else
			If Me.spikesActive Then
				MyBase.animator.Play("Spikes_Down")
				MyBase.StartCoroutine(Me.disableCollider_cr())
			Else
				MyBase.animator.Play("Off")
				Me.boxCollider.enabled = False
			End If
			Me.spikesActive = False
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06001C88 RID: 7304 RVA: 0x00104CAC File Offset: 0x001030AC
	Private Iterator Function disableCollider_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Spikes_Down", True, True)
		Me.boxCollider.enabled = False
		Return
	End Function

	' Token: 0x06001C89 RID: 7305 RVA: 0x00104CC7 File Offset: 0x001030C7
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001C8A RID: 7306 RVA: 0x00104CE5 File Offset: 0x001030E5
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x04002577 RID: 9591
	Private spriteRenderer As SpriteRenderer

	' Token: 0x04002578 RID: 9592
	Private damageDealer As DamageDealer

	' Token: 0x0400257A RID: 9594
	Private boxCollider As BoxCollider2D
End Class
