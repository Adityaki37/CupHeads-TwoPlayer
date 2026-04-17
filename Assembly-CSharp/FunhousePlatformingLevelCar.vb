Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008B1 RID: 2225
Public Class FunhousePlatformingLevelCar
	Inherits AbstractCollidableObject

	' Token: 0x060033DC RID: 13276 RVA: 0x001E16DB File Offset: 0x001DFADB
	Protected Overrides Sub Awake()
		MyBase.Awake()
		FunhousePlatformingLevelCar.CARS_ALIVE += 1
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x060033DD RID: 13277 RVA: 0x001E16FA File Offset: 0x001DFAFA
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060033DE RID: 13278 RVA: 0x001E1712 File Offset: 0x001DFB12
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060033DF RID: 13279 RVA: 0x001E1730 File Offset: 0x001DFB30
	Public Sub Init(pos As Vector2, rotation As Single, carSpeed As Single, index As Integer, leader As Boolean, last As Boolean)
		MyBase.transform.position = pos
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(rotation))
		Me.speed = carSpeed
		Me.leader = leader
		Me.last = last
		For Each gameObject As GameObject In Me.carSprites
			gameObject.SetActive(False)
		Next
		Me.carSprites(index).SetActive(True)
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060033E0 RID: 13280 RVA: 0x001E17CC File Offset: 0x001DFBCC
	Private Iterator Function move_cr() As IEnumerator
		If Me.leader Then
			AudioManager.PlayLoop("funhouse_car_idle")
			Me.emitAudioFromObject.Add("funhouse_car_idle")
		End If
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim size As Single = MyBase.GetComponent(Of Collider2D)().bounds.size.x
		While MyBase.transform.position.x > CupheadLevelCamera.Current.Bounds.xMin - (size + 50F)
			MyBase.transform.position += Vector3.left * Me.speed * CupheadTime.FixedDelta
			Yield wait
		End While
		If Me.last AndAlso FunhousePlatformingLevelCar.CARS_ALIVE <= 1 Then
			AudioManager.[Stop]("funhouse_car_idle")
		End If
		FunhousePlatformingLevelCar.CARS_ALIVE -= 1
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x04003C23 RID: 15395
	Private Shared CARS_ALIVE As Integer

	' Token: 0x04003C24 RID: 15396
	<SerializeField()>
	Private carSprites As GameObject()

	' Token: 0x04003C25 RID: 15397
	Private leader As Boolean

	' Token: 0x04003C26 RID: 15398
	Private last As Boolean

	' Token: 0x04003C27 RID: 15399
	Private speed As Single

	' Token: 0x04003C28 RID: 15400
	Private damageDealer As DamageDealer
End Class
