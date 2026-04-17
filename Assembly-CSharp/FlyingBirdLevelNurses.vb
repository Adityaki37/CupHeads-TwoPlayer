Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000625 RID: 1573
Public Class FlyingBirdLevelNurses
	Inherits AbstractCollidableObject

	' Token: 0x06001FFA RID: 8186 RVA: 0x00125B08 File Offset: 0x00123F08
	Public Sub InitNurse(properties As LevelProperties.FlyingBird.Nurses)
		Me.nurses = MyBase.transform.GetChildTransforms()
		For Each transform As Transform In Me.nurses
			transform.gameObject.SetActive(True)
		Next
		Me.leftSideShooting = Global.UnityEngine.Random.Range(-1, 1) >= 0
		Me.properties = properties
		Me.attackIndex = Global.UnityEngine.Random.Range(0, properties.attackCount.Split(New Char() { ","c }).Length)
		Me.pinkPattern = properties.pinkString.Split(New Char() { ","c })
		Me.pinkIndex = 0
		For Each transform2 As Transform In Me.nurses
			If transform2.GetComponent(Of Collider2D)() IsNot Nothing Then
				transform2.GetComponent(Of Collider2D)().enabled = True
			End If
		Next
		MyBase.StartCoroutine(Me.attack_cr())
	End Sub

	' Token: 0x06001FFB RID: 8187 RVA: 0x00125C0C File Offset: 0x0012400C
	Private Iterator Function attack_cr() As IEnumerator
		Dim shootLeft As Boolean = Rand.Bool()
		Dim multiplayer As Boolean = PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing
		While True
			Dim max As Integer = Parser.IntParse(Me.properties.attackCount.Split(New Char() { ","c })(Me.attackIndex))
			For i As Integer = 0 To max - 1
				If i <> 0 Then
					Yield CupheadTime.WaitForSeconds(Me, Me.properties.attackRepeatDelay)
				End If
				If shootLeft Then
					MyBase.animator.SetBool("ANurseATK", True)
				Else
					MyBase.animator.SetBool("BNurseATK", True)
				End If
				shootLeft = Not shootLeft
				If multiplayer Then
					Me.target += 1
					If Me.target > PlayerId.PlayerTwo Then
						Me.target = PlayerId.PlayerOne
					End If
				End If
			Next
			Me.leftSideShooting = Not Me.leftSideShooting
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.attackMainDelay)
			Me.attackIndex += 1
			If Me.attackIndex >= Me.properties.attackCount.Split(New Char() { ","c }).Length Then
				Me.attackIndex = 0
			End If
		End While
		Return
	End Function

	' Token: 0x06001FFC RID: 8188 RVA: 0x00125C28 File Offset: 0x00124028
	Private Sub ShootLeft()
		Me.spitFXLeft.SetActive(False)
		Dim abstractProjectile As AbstractProjectile = Me.pillPrefab.Create(Me.shootLeftPosRoot.position + MyBase.transform.up.normalized * 0.1F)
		abstractProjectile.GetComponent(Of FlyingBirdLevelNursePill)().InitPill(Me.properties, Me.target, Me.pinkPattern(Me.pinkIndex) = "P")
		Me.pinkIndex = (Me.pinkIndex + 1) Mod Me.pinkPattern.Length
		MyBase.animator.SetBool("ANurseATK", False)
		Me.spitFXLeft.SetActive(True)
	End Sub

	' Token: 0x06001FFD RID: 8189 RVA: 0x00125CE4 File Offset: 0x001240E4
	Private Sub ShootRight()
		Me.spitFXRight.SetActive(False)
		Dim abstractProjectile As AbstractProjectile = Me.pillPrefab.Create(Me.shootRightPosRoot.position + MyBase.transform.up.normalized * 0.1F)
		abstractProjectile.GetComponent(Of FlyingBirdLevelNursePill)().InitPill(Me.properties, Me.target, Me.pinkPattern(Me.pinkIndex) = "P")
		Me.pinkIndex = (Me.pinkIndex + 1) Mod Me.pinkPattern.Length
		MyBase.animator.SetBool("BNurseATK", False)
		Me.spitFXRight.SetActive(True)
	End Sub

	' Token: 0x06001FFE RID: 8190 RVA: 0x00125D9D File Offset: 0x0012419D
	Private Sub ShootSFX()
		AudioManager.Play("nurse_attack")
		Me.emitAudioFromObject.Add("nurse_attack")
	End Sub

	' Token: 0x06001FFF RID: 8191 RVA: 0x00125DB9 File Offset: 0x001241B9
	Public Sub Die()
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x0400287C RID: 10364
	Private Const Regular As String = "R"

	' Token: 0x0400287D RID: 10365
	Private Const Parry As String = "P"

	' Token: 0x0400287E RID: 10366
	<SerializeField()>
	Private pillPrefab As AbstractProjectile

	' Token: 0x0400287F RID: 10367
	<SerializeField()>
	Private shootRightPosRoot As Transform

	' Token: 0x04002880 RID: 10368
	<SerializeField()>
	Private shootLeftPosRoot As Transform

	' Token: 0x04002881 RID: 10369
	<SerializeField()>
	Private spitFXLeft As GameObject

	' Token: 0x04002882 RID: 10370
	<SerializeField()>
	Private spitFXRight As GameObject

	' Token: 0x04002883 RID: 10371
	Private leftSideShooting As Boolean

	' Token: 0x04002884 RID: 10372
	Private attackIndex As Integer

	' Token: 0x04002885 RID: 10373
	Private target As PlayerId

	' Token: 0x04002886 RID: 10374
	Private pinkPattern As String()

	' Token: 0x04002887 RID: 10375
	Private pinkIndex As Integer

	' Token: 0x04002888 RID: 10376
	Public nurses As Transform()

	' Token: 0x04002889 RID: 10377
	Private properties As LevelProperties.FlyingBird.Nurses
End Class
