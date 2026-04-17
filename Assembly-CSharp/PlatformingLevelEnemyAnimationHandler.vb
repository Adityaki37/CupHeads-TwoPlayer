Imports System
Imports UnityEngine

' Token: 0x02000903 RID: 2307
Public Class PlatformingLevelEnemyAnimationHandler
	Inherits AbstractPausableComponent

	' Token: 0x0600361D RID: 13853 RVA: 0x001F6F08 File Offset: 0x001F5308
	Public Sub SelectAnimation(type1 As String)
		For i As Integer = 0 To Me.numOfTypes - 1
			If type1.Substring(0, 1) = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(New Char() { ","c })(i) Then
				Me.index1 = i
			End If
			If Me.secondaryTypes > 0 AndAlso type1.Substring(1, 1) = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(New Char() { ","c })(i) Then
				Me.index2 = i + 1
			End If
		Next
		For Each spriteRenderer As SpriteRenderer In MyBase.GetComponentsInChildren(Of SpriteRenderer)()
			spriteRenderer.enabled = False
		Next
		MyBase.GetComponentsInChildren(Of SpriteRenderer)()(Me.index1).enabled = True
		If Me.secondaryTypes > 0 Then
			MyBase.GetComponent(Of Animator)().SetInteger("type", Me.index2)
		End If
	End Sub

	' Token: 0x04003E20 RID: 15904
	<SerializeField()>
	Private numOfTypes As Integer

	' Token: 0x04003E21 RID: 15905
	<SerializeField()>
	Private secondaryTypes As Integer

	' Token: 0x04003E22 RID: 15906
	Private index1 As Integer

	' Token: 0x04003E23 RID: 15907
	Private index2 As Integer

	' Token: 0x04003E24 RID: 15908
	Private Const LETTERS As String = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z"
End Class
