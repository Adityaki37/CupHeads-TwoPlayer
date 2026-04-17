Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200067C RID: 1660
Public Class FlyingGenieLevelSphinxPiece
	Inherits AbstractProjectile

	' Token: 0x1700039E RID: 926
	' (get) Token: 0x06002301 RID: 8961 RVA: 0x00148923 File Offset: 0x00146D23
	Protected Overrides ReadOnly Property DestroyedAfterLeavingScreen As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x06002302 RID: 8962 RVA: 0x00148928 File Offset: 0x00146D28
	Public Sub StartMoving(properties As LevelProperties.FlyingGenie.Sphinx, player As AbstractPlayerController, maxCounter As Integer, moveRight As Boolean, pinkPattern As String(), pinkIndex As Integer)
		Me.properties = properties
		Me.player = player
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Me.maxCounter = maxCounter
		Me.moveRight = moveRight
		Me.pinkPattern = pinkPattern
		Me.pinkIndex = pinkIndex
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002303 RID: 8963 RVA: 0x0014897C File Offset: 0x00146D7C
	Private Iterator Function move_cr() As IEnumerator
		MyBase.StartCoroutine(Me.spawn_minis_cr())
		While True
			MyBase.transform.position += MyBase.transform.right * Me.properties.sphinxSplitSpeed * CupheadTime.Delta * CSng(If((Not Me.moveRight), (-1), 1))
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002304 RID: 8964 RVA: 0x00148997 File Offset: 0x00146D97
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002305 RID: 8965 RVA: 0x001489B5 File Offset: 0x00146DB5
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002306 RID: 8966 RVA: 0x001489D4 File Offset: 0x00146DD4
	Private Iterator Function spawn_minis_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.miniInitialSpawnDelay)
		Dim counter As Integer = 0
		While MyBase.transform.position.y < 720F AndAlso MyBase.transform.position.y > -360F
			If counter >= Me.maxCounter Then
				Exit While
			End If
			Dim p As FlyingGenieLevelMiniCat = Me.miniCat.Create(MyBase.transform.position, 0F, Me.player, Me.properties)
			p.SetParryable(Me.pinkPattern(Me.pinkIndex)(0) = "P"c)
			Me.pinkIndex = (Me.pinkIndex + 1) Mod Me.pinkPattern.Length
			counter += 1
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.miniSpawnDelay)
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06002307 RID: 8967 RVA: 0x001489EF File Offset: 0x00146DEF
	Protected Overrides Sub RandomizeVariant()
	End Sub

	' Token: 0x06002308 RID: 8968 RVA: 0x001489F1 File Offset: 0x00146DF1
	Protected Overrides Sub SetTrigger(trigger As String)
	End Sub

	' Token: 0x04002B9C RID: 11164
	<SerializeField()>
	Private miniCat As FlyingGenieLevelMiniCat

	' Token: 0x04002B9D RID: 11165
	Private properties As LevelProperties.FlyingGenie.Sphinx

	' Token: 0x04002B9E RID: 11166
	Private player As AbstractPlayerController

	' Token: 0x04002B9F RID: 11167
	Private moveRight As Boolean

	' Token: 0x04002BA0 RID: 11168
	Private maxCounter As Integer

	' Token: 0x04002BA1 RID: 11169
	Private pinkPattern As String()

	' Token: 0x04002BA2 RID: 11170
	Private pinkIndex As Integer
End Class
