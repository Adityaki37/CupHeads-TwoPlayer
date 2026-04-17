Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200065C RID: 1628
Public Class FlyingCowboyLevelSausageLink
	Inherits BasicProjectile

	' Token: 0x060021EC RID: 8684 RVA: 0x0013C1C8 File Offset: 0x0013A5C8
	Public Sub Initialize(sausageType As FlyingCowboyLevelMeat.SausageType, sausageLinkSqueezePoint As Transform, previousLink As FlyingCowboyLevelSausageLink)
		Me.sausageType = sausageType
		If sausageType <> FlyingCowboyLevelMeat.SausageType.U1 AndAlso sausageType <> FlyingCowboyLevelMeat.SausageType.U2 AndAlso sausageType <> FlyingCowboyLevelMeat.SausageType.U3 Then
			MyBase.StartCoroutine(Me.squeeze_cr(sausageLinkSqueezePoint, previousLink))
		End If
		If sausageType = FlyingCowboyLevelMeat.SausageType.H1 OrElse sausageType = FlyingCowboyLevelMeat.SausageType.H2 OrElse sausageType = FlyingCowboyLevelMeat.SausageType.H3 OrElse sausageType = FlyingCowboyLevelMeat.SausageType.H4 OrElse sausageType = FlyingCowboyLevelMeat.SausageType.L5 Then
			MyBase.animator.SetFloat("Speed", CSng(Rand.PosOrNeg()))
		End If
	End Sub

	' Token: 0x060021ED RID: 8685 RVA: 0x0013C238 File Offset: 0x0013A638
	Public Sub Squeeze()
		MyBase.animator.Play("Squeeze" + Me.sausageType.ToString())
	End Sub

	' Token: 0x060021EE RID: 8686 RVA: 0x0013C260 File Offset: 0x0013A660
	Private Iterator Function squeeze_cr(sausageLinkSqueezePoint As Transform, previousLink As FlyingCowboyLevelSausageLink) As IEnumerator
		While MyBase.transform.position.x > sausageLinkSqueezePoint.position.x
			Yield Nothing
		End While
		Me.Squeeze()
		If previousLink IsNot Nothing Then
			previousLink.Squeeze()
		End If
		Return
	End Function

	' Token: 0x04002AA6 RID: 10918
	Private sausageType As FlyingCowboyLevelMeat.SausageType
End Class
