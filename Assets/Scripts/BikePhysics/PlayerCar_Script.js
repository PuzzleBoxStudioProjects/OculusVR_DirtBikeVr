

// These variables allow the script to power the wheels of the car.
var frontWheel : WheelCollider;
var backWheel : WheelCollider;

// These variables are for the gears, the array is the list of ratios. The script
// uses the defined gear ratios to determine how much torque to apply to the wheels.
var GearRatio : float[];
var CurrentGear : int = 0;

// These variables are just for applying torque to the wheels and shifting gears.
// using the defined Max and Min Engine RPM, the script can determine what gear the
// car needs to be in.
var EngineTorque : float = 600.0;
var MaxEngineRPM : float = 3000.0;
var MinEngineRPM : float = 1000.0;
private var EngineRPM : float = 0.0;



function Start () {
	// I usually alter the center of mass to make the car more stable. I'ts less likely to flip this way.
	rigidbody.centerOfMass.y =0;
}

function Update () {
	
	// This is to limith the maximum speed of the car, adjusting the drag probably isn't the best way of doing it,
	// but it's easy, and it doesn't interfere with the physics processing.
	rigidbody.drag = rigidbody.velocity.magnitude / 250;
	
	// Compute the engine RPM based on the average RPM of the two wheels, then call the shift gear function
	EngineRPM = (backWheel.rpm)/2 * GearRatio[CurrentGear];
	ShiftGears();

	// set the audio pitch to the percentage of RPM to the maximum RPM plus one, this makes the sound play
	// up to twice it's pitch, where it will suddenly drop when it switches gears.
//	audio.pitch = Mathf.Abs(EngineRPM / MaxEngineRPM) + 1.0 ;
	// this line is just to ensure that the pitch does not reach a value higher than is desired.
	//if ( audio.pitch > 2.0 ) {
		//audio.pitch = 2.0;
	//}

	// finally, apply the values to the wheels.	The torque applied is divided by the current gear, and
	// multiplied by the user input variable.
	backWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * Input.GetAxis("Vertical");
	
		
	// the steer angle is an arbitrary value multiplied by the user input.
	frontWheel.steerAngle = 10 * Input.GetAxis("Horizontal");
	
}

function ShiftGears() {
	// this funciton shifts the gears of the vehcile, it loops through all the gears, checking which will make
	// the engine RPM fall within the desired range. The gear is then set to this "appropriate" value.
	if ( EngineRPM >= MaxEngineRPM ) {
		var AppropriateGear : int = CurrentGear;
		
		for ( var i = 0; i < GearRatio.length; i ++ ) {
			if ( backWheel.rpm * GearRatio[i] < MaxEngineRPM ) {
				AppropriateGear = i;
				break;
			}
		}
		
		CurrentGear = AppropriateGear;
	}
	
	if ( EngineRPM <= MinEngineRPM ) {
		AppropriateGear = CurrentGear;
		
		for ( var j = GearRatio.length-1; j >= 0; j -- ) {
			if ( backWheel.rpm * GearRatio[j] > MinEngineRPM ) {
				AppropriateGear = j;
				break;
			}
		}
		
		CurrentGear = AppropriateGear;
	}
}