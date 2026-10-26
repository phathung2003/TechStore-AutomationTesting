<?php

namespace App\Http\Controllers\Admin;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\Category;
class CategoryController extends Controller
{
    //
    public function getList(){
        $title = 'List of Categories';
        $list = Category::all();
        if(!empty($list)) {
            return view('admin.category.list', compact('title','list'));
        }
        return view('admin.category.list',compact('title'));
    }
    public function getAdd(){
        $title = 'Add a new category';
        return view('admin.category.add', compact('title'));
    }
    public function postAdd(Request $req){
        $req ->validate([
            'name' => 'required | unique:brands',
            'description' => 'required | string'
        ]);
        $category = new Category;
        $category->name = $req->name;
        $category->description = $req->description;
        $check = $category->save();
        if($check) {
            return redirect()->route('admin.categories.getList',compact('check'));
        }
        return redirect()->route('admin.categories.getList');

    }
    public function getUpdate($id = 0){
        if(!empty($id) && ctype_digit($id)){
            $category = Category::find($id);
            if(!empty($category)) {
                return view('admin.category.update', compact('category'));
            }
        }
        return view('admin.category.update');
    }
    public function postUpdate(Request $req, $id){
        if(!empty($id) && ctype_digit($id)){
            $req ->validate([
                'name' => 'required | unique:categories,name,'.$id,
                'description' => 'required | string',
            ]);
            $category = Category::find($id);
            if(!empty($category)) {
                $category->name = $req->name;
                $category->description = $req->description;
                $check = $category->save();
                if($check) {
                    return redirect()->route('admin.categories.getList',compact('check'));
                }
            }
        }
        return redirect()->route('admin.categories.getList');
    }
    public function deleteItem($id = 0){
        if(!empty($id) && ctype_digit($id)){
            $category = Category::find($id);
            if(!empty($category)) {
                $check = $category->delete();
                if($check) {
                    return redirect()->route('admin.categories.getList',compact('check'));
                }
            }
        }
        return redirect()->route('admin.categories.getList');
    }
}
