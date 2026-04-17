Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000704 RID: 1796
Public Class OldManLevelGoose
	Inherits AbstractProjectile

	' Token: 0x0600269D RID: 9885 RVA: 0x001698F4 File Offset: 0x00167CF4
	Public Overridable Function Init(pos As Vector2, speed As Single, properties As LevelProperties.OldMan.GooseAttack, hasCollision As Boolean, sortingLayer As String, sortingOrder As Integer, whiten As Single) As OldManLevelGoose
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = pos
		Me.properties = properties
		Me.speed = speed
		Me.coll.enabled = hasCollision
		Me.rend.sortingLayerName = sortingLayer
		Me.rend.color = New Color(whiten, whiten, whiten)
		If sortingLayer = "Foreground" Then
			Me.rend.material = Me.altMaterial
			MyBase.gameObject.layer = 31
		End If
		Me.rend.sortingOrder = sortingOrder
		Me.anim.Play((Global.UnityEngine.Random.Range(0, 8) Mod 6).ToString())
		Me.Move()
		Return Me
	End Function

	' Token: 0x0600269E RID: 9886 RVA: 0x001699C1 File Offset: 0x00167DC1
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600269F RID: 9887 RVA: 0x001699DF File Offset: 0x00167DDF
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060026A0 RID: 9888 RVA: 0x001699FD File Offset: 0x00167DFD
	Private Sub Move()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060026A1 RID: 9889 RVA: 0x00169A0C File Offset: 0x00167E0C
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While MyBase.transform.position.x > CSng(Level.Current.Left) - 1000F
			MyBase.transform.position += Vector3.left * Me.speed * CupheadTime.FixedDelta
			Yield wait
		End While
		Me.Recycle()
		Yield Nothing
		Return
	End Function

	' Token: 0x04002F56 RID: 12118
	Private Const OFFSET As Single = 1000F

	' Token: 0x04002F57 RID: 12119
	Private properties As LevelProperties.OldMan.GooseAttack

	' Token: 0x04002F58 RID: 12120
	Private speed As Single

	' Token: 0x04002F59 RID: 12121
	<SerializeField()>
	Private coll As BoxCollider2D

	' Token: 0x04002F5A RID: 12122
	<SerializeField()>
	Private anim As Animator

	' Token: 0x04002F5B RID: 12123
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x04002F5C RID: 12124
	<SerializeField()>
	Private altMaterial As Material
End Class
