<?php
namespace App\Http\Controllers\API;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use App\Models\User;

class AuthController extends Controller
{
    public $successStatus = 200;
    public function register(Request $req) {
        $req->validate([
            'name' => 'required',
            'email' => 'required|email',
            'password' => 'required|min:6',
            'phone' => 'required',
            'address' => 'required',
        ]);

        $user = new User;
        $user->name = $req->name;
        $user->email = $req->email;
        $user->password = $req->password;
        $user->phone = $req->phone;
        $user->address = $req->address;
        $user->save();
        return response()->json($user,201);
    }
    public function login(Request $req) {
        $req->validate([
            'email' => 'email|required',
            'password' => 'required'
        ]);

        $credentials = request(['email', 'password']);
        if (!auth()->attempt($credentials)) {
            return response()->json([
                'message' => 'The given data was invalid.',
                'errors' => [
                    'password' => [
                        'Invalid credentials'
                    ],
                ]
            ], 422);
        }

        $user = User::where('email', $req->email)->first();
        $authToken = $user->createToken('auth-token')->plainTextToken;

        return response()->json([
            'access_token' => $authToken,
        ],200);
    }
    public function logout(Request $req) {
        $req->user()->currentAccessToken()->delete();
        if($req) {
            return response()->json([
                'status' => 'success'
            ],200);
        } else {
            return response()->json([
                'status' => 'error'
            ],400);
        }
    }
    public function getProfile(){
        return response()->json([
            'data' => Auth::user()
        ], 200);
    }
}
