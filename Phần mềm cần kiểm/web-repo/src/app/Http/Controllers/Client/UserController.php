<?php

namespace App\Http\Controllers\Client;

use App\Http\Controllers\Controller;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Hash;
use Illuminate\Http\Request;
use App\Models\User;

class UserController extends Controller
{
    //
    public function getUserProfile(){
        $path = 'Account Detail';
        return view('client.profile.index', compact('path'));
    }

    public function postUserProfile(Request $req, $id=0){
        $id = Auth::user()->id;
        $req ->validate([
            'name' => 'required | max:255 | string',
            'email' => 'required | email',
            'dob' => 'before:today'
        ]);
        if(!empty($id)){
            $user = User::find($id);
            if(!empty($user)){
                $user->name = $req->name;
                $user->email = $req->email;
                $user->phone = $req->phone;
                $user->dob = $req->dob;
                $user->gender = $req->gender;
                $user->address = $req->address;
                $check = $user->save();
                if($check){
                    return redirect()->route('getUserProfile')->with('success', 'Your information has been changed.');
                }
            }
        }
        return redirect()->route('getUserProfile')->with('error', "Can't find user");
    }

    public function getUserOrder(){
        $path = 'Order';
        return view('client.profile.order', compact('path'));
    }
    public function getChangePassword(){
        $path = 'Change Password';
        return view('client.profile.password', compact('path'));
    }

    public function postChangePassword(Request $req, $id=0){
        $id = Auth::user()->id;

        $req->validate([
            'currentPassword' => 'required | min:6',
            'newPassword' => 'required | min:6 | different:currentPassword',
            'confirmPassword' => 'required | same:newPassword'
        ]);

        if(!empty($id)){
            $user = User::find($id);
            if(!empty($user)){
                $currentPassword = $req->input('currentPassword');
                $newPassword = $req->input('newPassword');
                $confirmPassword = $req->input('confirmPassword');

                if(Hash::check($currentPassword, $user->password)){
                    $user->password = bcrypt($newPassword);
                    $check = $user->save();
                    if($check){
                        return redirect()->route('changePassword')->with('success', 'Your password has been changed.');
                    }
                }
            }
            return redirect()->route('changePassword')->with('error', 'Current password is incorrect.');
        }
    }
}
